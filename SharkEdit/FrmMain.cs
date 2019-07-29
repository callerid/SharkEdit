using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharkEdit
{
    public partial class FrmMain : Form
    {
        // Wireshark object for processing and saving
        private WireSharkFile WireSharkPackets;

        // Keep track of first loaded file
        private bool FirstLoad = true;
        
        // DDV constants
        private const int DGV_DISPLAY_COUNT = 0;
        private const int DGV_DISPLAY_TIMESTAMP = 1;
        private const int DGV_DISPLAY_LENGTH = 2;
        private const int DGV_DISPLAY_SIP_TYPE = 3;


        // Form start
        public FrmMain()
        {
            // Load components
            InitializeComponent();
        }

        // Click of load file button
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            // Create and show dialog to get filepath for 
            // desired file
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "Cap Files|*.cap";
            file_dialog.ShowDialog();

            // Load file then process
            LoadFile(file_dialog.FileName, FirstLoad ? false : true);
        }

        private void LoadFile(string filename, bool confirm_message)
        {
            // Display message to make sure user wants to clear
            // all old data and re-load file
            if (confirm_message)
            {
                if (MessageBox.Show("Are you sure you wish to clear all unsaved changes?", "Re-load New File?", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            // No longer first load
            FirstLoad = false;

            // Process File
            ProcessFile(filename);

        }

        private void ProcessFile(string filename)
        {
            lbPackets.Text = "Reading File...";
            pbLoading.Value = 0;
            WaitFor(75);
            byte[] full_wireshark_file = File.ReadAllBytes(filename);
            lbPackets.Text = "Processing...";
            WaitFor(75);

            // Make copy of current file for editing
            // start at index 24 which will remove
            // WireShark's global header since we only
            // care about the packets
            byte[] file_contents = new byte[full_wireshark_file.Length - 23];
            for(int i = 0; i < full_wireshark_file.Length - 24; i++)
            {
                file_contents[i] = full_wireshark_file[i + 24];
            }

            byte[] g_header = new byte[24];
            for(int i = 0; i < 24; i++)
            {
                g_header[i] = full_wireshark_file[i];
            }

            // Create wireshark file object keeper
            WireSharkPackets = new WireSharkFile(file_contents, g_header);

            // Connect updaters ( used for progress bars )
            WireSharkPackets.SetProgressBarMax += new WireSharkFile.SetProgressBarMaxDelegate(SetProgressBarMax);
            WireSharkPackets.UpdateProgressBar += new WireSharkFile.UpdateProgressBarDelegate(UpdateProgressBar);

            // Get all lengths and timestamps (pre-process)
            WireSharkPackets.Process(true, true, true, true);

            // Update GUI
            lbPackets.Text = "Displaying...";
            WaitFor(75);
            RefreshDisplay();

        }

        private void SetProgressBarMax(int value)
        {
            pbLoading.Maximum = value;
        }

        private void UpdateProgressBar(int value)
        {
            if (value < pbLoading.Maximum) pbLoading.Value = value;
        }

        // Update GUI
        public void RefreshDisplay()
        {
            // Clear all old values
            dgvDisplay.Rows.Clear();

            if (MessageBox.Show("Display?", "Update Display?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                pbLoading.Value = 0;
                return;
            }

            // Populate display
            pbLoading.Maximum = WireSharkPackets.GetPacketsCount();
            pbLoading.Value = 1;
            for (int i = 0; i < WireSharkPackets.GetPacketsCount(); i++)
            {
                // Update display
                dgvDisplay.Rows.Add();
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_COUNT].Value = i + 1;

                int[] timestamps = WireSharkPackets.GetPacketTimestamp(i);
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_TIMESTAMP].Value = timestamps[0] + "." + timestamps[1];

                int packet_length = WireSharkPackets.GetPacketLength(i);
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_LENGTH].Value = packet_length;

                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_SIP_TYPE].Value = WireSharkPackets.GetSIPString(i);

                if (pbLoading.Value < pbLoading.Maximum) pbLoading.Value++;

            }

            lbPackets.Text = "Packets: " + WireSharkPackets.GetPacketsCount();
            string runtime = dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_TIMESTAMP].Value.ToString();
            lbTotalTime.Text = "Total Time: ~(" + int.Parse(runtime.Substring(0, runtime.IndexOf('.')))/60 + ") Mins <==> " + 
                                runtime.Substring(0, runtime.IndexOf('.')) + " s and " + runtime.Substring(runtime.IndexOf('.') + 1) + " ms";
            pbLoading.Value = 0;
        }

        private void AlterTimes(int milliseconds_between_sip, int rtp_divider)
        {
            if (WireSharkPackets == null) return;

            // Correct for wireshark format
            milliseconds_between_sip = milliseconds_between_sip * 1000;            

            lbPackets.Text = "Altering times...";
            WaitFor(75);

            int timestamp = 0;
            long u_timestamp = 0;
            SetProgressBarMax(WireSharkPackets.GetPacketsCount());
            int progress = 0;
            for(int i = 0; i < WireSharkPackets.GetPacketsCount(); i++)
            {
                byte[] timestamp_bytes = new byte[4];
                byte[] u_timestamp_bytes = new byte[4];

                // Convert timestamps to byte arrays
                timestamp_bytes = BitConverter.GetBytes(timestamp);
                u_timestamp_bytes = BitConverter.GetBytes(u_timestamp);

                // Set new values
                WireSharkPackets.SetPacketTimestamp(i, timestamp_bytes, u_timestamp_bytes);

                // Increase by given value
                int increment_by = milliseconds_between_sip;
                bool is_rtp = WireSharkPackets.IsRTP(i);

                double r = milliseconds_between_sip / rtp_divider;
                if (is_rtp) increment_by = (int)Math.Round(r);

                // Increase next timestamp
                u_timestamp += increment_by;
                if (u_timestamp >= 1000000)
                {
                    double remaining_seconds = u_timestamp / 1000000;
                    int seconds = (int)Math.Round(remaining_seconds);
                    timestamp+= seconds;

                    float remaining_u_seconds = u_timestamp / 1000000;
                    remaining_u_seconds = (float)GetPartialFromFloat(remaining_u_seconds) * 1000;
                    u_timestamp = (long)Math.Round(remaining_u_seconds);
                }

                progress++;
                UpdateProgressBar(progress);
            }

            RefreshDisplay();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Cap Files|*.cap";
            if(save_dialog.ShowDialog()== DialogResult.OK)
            {
                if (WireSharkPackets.WriteFile(save_dialog.FileName))
                {
                    MessageBox.Show("Exported.");
                }
                else
                {
                    MessageBox.Show("Error while exporting. Please retry.");
                }

                pbLoading.Value = 0;

            }
        }

        // Wait for x miliseconds
        public static void WaitFor(int milliSeconds)
        {
            var sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < milliSeconds)
            {
                Application.DoEvents();
            }
            sw.Stop();
        }

        public static decimal GetPartialFromFloat(float f)
        {
            return (decimal)(f - Math.Truncate(f));
        }

        private void btnHalfSecondFix_Click(object sender, EventArgs e)
        {
            AlterTimes(100, 50);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            Common.WaitFor(75);
            byte[] full_wireshark_file = File.ReadAllBytes(filename);
            lbPackets.Text = "Processing...";
            Common.WaitFor(75);

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
            WireSharkPackets.Process();

            // Update GUI
            lbPackets.Text = "Displaying...";
            Common.WaitFor(75);
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

                if (pbLoading.Value < pbLoading.Maximum) pbLoading.Value++;

            }

            lbPackets.Text = "Packets: " + WireSharkPackets.GetPacketsCount();
            pbLoading.Value = 0;
        }

        private void btnHalfSecondFix_Click(object sender, EventArgs e)
        {
            if (WireSharkPackets == null) return;
            int timestamp = 0;
            long u_timestamp = 0;
            for(int i = 0; i < WireSharkPackets.GetPacketsCount(); i++)
            {
                byte[] timestamp_bytes = new byte[4];
                byte[] u_timestamp_bytes = new byte[4];

                // Convert timestamps to byte arrays
                timestamp_bytes = BitConverter.GetBytes(timestamp);
                u_timestamp_bytes = BitConverter.GetBytes(u_timestamp);

                // Set new values
                WireSharkPackets.SetPacketTimestamp(i, timestamp_bytes, u_timestamp_bytes);

                // Increase by half a second
                if (u_timestamp < 500000)
                {
                    u_timestamp += 500000;
                }
                else
                {
                    u_timestamp = 0;
                    timestamp++;
                }
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

            }
        }
    }
}

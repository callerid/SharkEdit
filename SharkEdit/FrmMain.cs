﻿using System;
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
        private bool TimesAltered = false;
        
        // DDV constants
        private const int DGV_DISPLAY_COUNT = 0;
        private const int DGV_DISPLAY_TIMESTAMP = 1;
        private const int DGV_DISPLAY_LENGTH = 2;
        private const int DGV_DISPLAY_SIP_TYPE = 3;
        private const int DGV_DISPLAY_RAW = 4;


        // Form start
        public FrmMain()
        {
            // Load components
            InitializeComponent();
            Text = "Shark Byte " + ProductVersion.ToString();
        }

        // Click of load file button
        private void btnLoadFile_Click(object sender, EventArgs e)
        {

            // Display message to make sure user wants to clear
            // all old data and re-load file
            if (TimesAltered)
            {
                if (MessageBox.Show("Export Existing File Created?", "Export File?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btnExport_Click(new object(), new EventArgs());
                    return;
                }
            }

            // Create and show dialog to get filepath for 
            // desired file
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "Cap Files|*.cap";
            file_dialog.ShowDialog();

            // Load file then process
            if (string.IsNullOrEmpty(file_dialog.FileName)) return;
            LoadFile(file_dialog.FileName);
            btnHalfSecondFix.Enabled = true;
        }

        private void LoadFile(string filename)
        {
            lbFilename.Text = filename;

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
            WireSharkPackets.Process(ckbNonEssential.Checked, ckbOPT.Checked, ckbRTP.Checked);

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
            
            // Populate display
            pbLoading.Maximum = WireSharkPackets.GetPacketsCount();
            if (pbLoading.Maximum > 0) pbLoading.Value = 1;
            for (int i = 0; i < WireSharkPackets.GetPacketsCount(); i++)
            {
                // Update display
                dgvDisplay.Rows.Add();
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_COUNT].Value = i + 1;

                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_TIMESTAMP].Value = WireSharkPackets.GetPacketTimestamp(i);

                int packet_length = WireSharkPackets.GetPacketLength(i);
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_LENGTH].Value = packet_length;

                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_SIP_TYPE].Value = WireSharkPackets.GetSIPString(i);

                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_RAW].Value = Encoding.ASCII.GetString(WireSharkPackets.GetPacket(i));

                if (pbLoading.Value < pbLoading.Maximum) pbLoading.Value++;

            }

            lbPackets.Text = "Packets: " + WireSharkPackets.GetPacketsCount();
            pbLoading.Value = 0;
        }

        private void AlterTimes(int milliseconds_between_sip, int rtp_divider)
        {
            if (WireSharkPackets == null) return;

            // Correct for wireshark format
            WireSharkPackets.SetStartTime(0);
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

                if (is_rtp) increment_by = (int)ndMSBR.Value * 1000;

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
                    pbLoading.Value = 0;
                    MessageBox.Show("Export Complete.");
                    btnExport.Enabled = false;
                    btnHalfSecondFix.Enabled = false;
                    TimesAltered = false;

                    ckbOPT.Enabled = true;
                    ckbNonEssential.Enabled = true;
                    ckbRTP.Enabled = true;

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
            AlterTimes((int)ndMSBetweenSIP.Value, (int)ndMSBR.Value);
            TimesAltered = true;
            btnExport.Enabled = true;

            ckbOPT.Enabled = false;
            ckbNonEssential.Enabled = false;
            ckbRTP.Enabled = false;
        }

        private void ckbRTP_CheckedChanged(object sender, EventArgs e)
        {
            ndMSBR.Enabled = !ckbRTP.Checked;
            lbRTP.Enabled = !ckbRTP.Checked;

        }

        private void label2_Click(object sender, EventArgs e)
        {
            string list = Environment.NewLine +
                " -- Register " + Environment.NewLine +
                //" -- Invite " + Environment.NewLine +
                //" -- 100 Trying " + Environment.NewLine +
                //" -- 180 Ringing " + Environment.NewLine +
                " -- Acknowledgment " + Environment.NewLine +
                //" -- Bye " + Environment.NewLine +
                " -- 200 OK " + Environment.NewLine +
                //" -- Options " + Environment.NewLine +
                //" -- Cancel " + Environment.NewLine +
                " -- Notify " + Environment.NewLine +
                " -- 401 Unauthorized " + Environment.NewLine +
                " -- Subscribe " + Environment.NewLine +
                " -- Not Acceptable " + Environment.NewLine +
                " -- Accepted " + Environment.NewLine +
                " -- PRACK " + Environment.NewLine +
                " -- Request Cancelled " + Environment.NewLine +
                //" -- Proxy Auth. " + Environment.NewLine +
                " -- Info ";

            FrmFiltering fFiltering = new FrmFiltering(list);
            fFiltering.ShowDialog();
        }
    }
}

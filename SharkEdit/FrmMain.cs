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
        // Buffer for Loaded File (pre-changes)
        public byte[] CurrentFileContents;

        // Buffer for Loaded File (post-changes)
        public byte[] FileContents;

        // Wireshark object for processing and saving
        private WireSharkFile WireSharkPackets;

        // Keep track of first loaded file
        private bool FirstLoad = true;
        
        // DDV constants
        private const int DGV_DISPLAY_COUNT = 0;
        private const int DGV_DISPLAY_TIMESTAMP = 1;
        private const int DGV_DISPLAY_LENGTH = 2;
        private const int DGV_DISPLAY_INTERVAL = 3;
        private const int DGV_DISPLAY_DELAY_AFTER = 4;


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

            // Clear all old values
            dgvDisplay.Rows.Clear();

            // Clear buffers (precaution, they should be 
            // cleared when loaded anyway
            CurrentFileContents = new byte[1];
            FileContents = new byte[1];

            // Erase old file and load new file
            CurrentFileContents = File.ReadAllBytes(filename);
            FileContents = File.ReadAllBytes(filename);

            // Process File
            ProcessFile();

        }

        private void ProcessFile()
        {

            // Make copy of current file for editing
            // start at index 24 which will remove
            // WireShark's global header since we only
            // care about the packets
            byte[] file_contents = new byte[CurrentFileContents.Length - 23];
            for(int i = 0; i < CurrentFileContents.Length - 24; i++)
            {
                file_contents[i] = CurrentFileContents[i + 24];
            }

            // Create wireshark file object keeper
            WireSharkPackets = new WireSharkFile(file_contents);

            // Populate display
            for(int i = 0; i < WireSharkPackets.GetPacketsCount(); i++)
            {
                // Update display
                dgvDisplay.Rows.Add();
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_COUNT].Value = i + 1;

                int[] timestamps = WireSharkPackets.GetPacketTimestamp(i);
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_TIMESTAMP].Value = timestamps[0] + "." + timestamps[1];

                int packet_length = WireSharkPackets.GetPacketLength(i);
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_LENGTH].Value = packet_length;

            }            

            lbPackets.Text = "Packets: " + WireSharkPackets.GetPacketsCount();

        }
    }
}

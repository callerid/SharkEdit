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

        // Keep track of first loaded file
        private bool FirstLoad = true;

        // Contants
        private const int HEADER_SIZE = 16;

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
            
            // Prepare for packet separation
            List<byte[]> Packets = new List<byte[]>();

            // Loop through all packets and pull them out
            int bytes_left = file_contents.Length;
            int packet_count = 1;
            while (bytes_left > 1)
            {
                // Header Total Length = 16 bytes
                // Header[0]-[3] => Time Stamps
                // Header[4]-[7] => Time Intervals
                // Header[8]-[11] => Length of Packet
                // Header[12]-[15] => Length of Packet (again)

                // Get length of the packet
                byte[] length_bytes = new byte[4] { file_contents[8], file_contents[9] , file_contents[10], file_contents[11] };
                int packet_length = BitConverter.ToInt32(length_bytes, 0);

                // Update counter to watch of end of file
                bytes_left = file_contents.Length - (packet_length + HEADER_SIZE);

                // Refresh contents to stay up-to-date with loop
                byte[] buffer = new byte[bytes_left];
                for(int i = 0; i < bytes_left; i++)
                { 
                    buffer[i] = file_contents[i + packet_length + HEADER_SIZE];
                }
                file_contents = new byte[bytes_left];
                buffer.CopyTo(file_contents, 0);

                // Update display
                dgvDisplay.Rows.Add();
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_COUNT].Value = packet_count;
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_LENGTH].Value = packet_length;
                dgvDisplay.Rows[dgvDisplay.Rows.Count - 1].Cells[DGV_DISPLAY_DELAY_AFTER].Value = "";

                // Count packets
                if(bytes_left > 1) packet_count++;

            }

            lbPackets.Text = "Packets: " + packet_count;

        }
    }
}

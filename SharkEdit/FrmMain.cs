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

            ProcessFile(filename);

        }

        private void ProcessFile(string filename)
        {

            // Break file into packets
            List<byte[]> Packets = new List<byte[]>();

            // Loop through all packets

            // Header Total Lenght = 15 bytes

            // Header[0]-[3] => Time Stamps

            // Header[4]-[7] => Time Intervals

            // Header[8]-[11] => Length of Packet

            // Header[12]-[15] => Lenght of Packet (again)

        }
    }
}

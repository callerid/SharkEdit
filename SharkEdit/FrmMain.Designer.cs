﻿namespace SharkEdit
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.dgvDisplay = new System.Windows.Forms.DataGridView();
            this.dgvDisplayColPacketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColSIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.lbPackets = new System.Windows.Forms.Label();
            this.btnHalfSecondFix = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.ndMSBetweenSIP = new System.Windows.Forms.NumericUpDown();
            this.lbMSBS = new System.Windows.Forms.Label();
            this.ckbRTP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbREG = new System.Windows.Forms.CheckBox();
            this.ckbOK = new System.Windows.Forms.CheckBox();
            this.ckbOPT = new System.Windows.Forms.CheckBox();
            this.ckbNOT = new System.Windows.Forms.CheckBox();
            this.lbRTP = new System.Windows.Forms.Label();
            this.ndMSBR = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lbFilename = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMSBetweenSIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMSBR)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDisplay
            // 
            this.dgvDisplay.AllowUserToAddRows = false;
            this.dgvDisplay.AllowUserToDeleteRows = false;
            this.dgvDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDisplayColPacketCount,
            this.dgvDisplayColTimeStamp,
            this.dgvDisplayColLength,
            this.dgvDisplayColSIP});
            this.dgvDisplay.Location = new System.Drawing.Point(12, 104);
            this.dgvDisplay.Name = "dgvDisplay";
            this.dgvDisplay.ReadOnly = true;
            this.dgvDisplay.RowHeadersVisible = false;
            this.dgvDisplay.Size = new System.Drawing.Size(545, 444);
            this.dgvDisplay.TabIndex = 0;
            // 
            // dgvDisplayColPacketCount
            // 
            this.dgvDisplayColPacketCount.HeaderText = "#";
            this.dgvDisplayColPacketCount.Name = "dgvDisplayColPacketCount";
            this.dgvDisplayColPacketCount.ReadOnly = true;
            this.dgvDisplayColPacketCount.Width = 35;
            // 
            // dgvDisplayColTimeStamp
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvDisplayColTimeStamp.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDisplayColTimeStamp.HeaderText = "Timestamp";
            this.dgvDisplayColTimeStamp.Name = "dgvDisplayColTimeStamp";
            this.dgvDisplayColTimeStamp.ReadOnly = true;
            this.dgvDisplayColTimeStamp.Width = 175;
            // 
            // dgvDisplayColLength
            // 
            this.dgvDisplayColLength.HeaderText = "Length";
            this.dgvDisplayColLength.Name = "dgvDisplayColLength";
            this.dgvDisplayColLength.ReadOnly = true;
            this.dgvDisplayColLength.Width = 55;
            // 
            // dgvDisplayColSIP
            // 
            this.dgvDisplayColSIP.HeaderText = "SIP Type";
            this.dgvDisplayColSIP.Name = "dgvDisplayColSIP";
            this.dgvDisplayColSIP.ReadOnly = true;
            this.dgvDisplayColSIP.Width = 115;
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 75);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(238, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "#1: Load File with Selected Filters";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // lbPackets
            // 
            this.lbPackets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbPackets.AutoSize = true;
            this.lbPackets.Location = new System.Drawing.Point(389, 580);
            this.lbPackets.Name = "lbPackets";
            this.lbPackets.Size = new System.Drawing.Size(58, 13);
            this.lbPackets.TabIndex = 2;
            this.lbPackets.Text = "Packets: 0";
            // 
            // btnHalfSecondFix
            // 
            this.btnHalfSecondFix.Enabled = false;
            this.btnHalfSecondFix.Location = new System.Drawing.Point(375, 75);
            this.btnHalfSecondFix.Name = "btnHalfSecondFix";
            this.btnHalfSecondFix.Size = new System.Drawing.Size(182, 23);
            this.btnHalfSecondFix.TabIndex = 3;
            this.btnHalfSecondFix.Text = "#2: Set Packet Intervals";
            this.btnHalfSecondFix.UseVisualStyleBackColor = true;
            this.btnHalfSecondFix.Click += new System.EventHandler(this.btnHalfSecondFix_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(12, 575);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(116, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "#3: Export to File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // pbLoading
            // 
            this.pbLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbLoading.Location = new System.Drawing.Point(134, 575);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(249, 23);
            this.pbLoading.TabIndex = 5;
            // 
            // ndMSBetweenSIP
            // 
            this.ndMSBetweenSIP.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ndMSBetweenSIP.Location = new System.Drawing.Point(375, 49);
            this.ndMSBetweenSIP.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ndMSBetweenSIP.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ndMSBetweenSIP.Name = "ndMSBetweenSIP";
            this.ndMSBetweenSIP.Size = new System.Drawing.Size(65, 20);
            this.ndMSBetweenSIP.TabIndex = 7;
            this.ndMSBetweenSIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndMSBetweenSIP.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lbMSBS
            // 
            this.lbMSBS.AutoSize = true;
            this.lbMSBS.Location = new System.Drawing.Point(389, 29);
            this.lbMSBS.Name = "lbMSBS";
            this.lbMSBS.Size = new System.Drawing.Size(24, 13);
            this.lbMSBS.TabIndex = 8;
            this.lbMSBS.Text = "SIP";
            // 
            // ckbRTP
            // 
            this.ckbRTP.AutoSize = true;
            this.ckbRTP.Checked = true;
            this.ckbRTP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbRTP.Location = new System.Drawing.Point(12, 28);
            this.ckbRTP.Name = "ckbRTP";
            this.ckbRTP.Size = new System.Drawing.Size(147, 17);
            this.ckbRTP.TabIndex = 9;
            this.ckbRTP.Text = "RTP (unchecked 1 of 25)";
            this.ckbRTP.UseVisualStyleBackColor = true;
            this.ckbRTP.CheckedChanged += new System.EventHandler(this.ckbRTP_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Filtered Packets";
            // 
            // ckbREG
            // 
            this.ckbREG.AutoSize = true;
            this.ckbREG.Checked = true;
            this.ckbREG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbREG.Location = new System.Drawing.Point(165, 29);
            this.ckbREG.Name = "ckbREG";
            this.ckbREG.Size = new System.Drawing.Size(65, 17);
            this.ckbREG.TabIndex = 11;
            this.ckbREG.Text = "Register";
            this.ckbREG.UseVisualStyleBackColor = true;
            // 
            // ckbOK
            // 
            this.ckbOK.AutoSize = true;
            this.ckbOK.Checked = true;
            this.ckbOK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbOK.Location = new System.Drawing.Point(165, 52);
            this.ckbOK.Name = "ckbOK";
            this.ckbOK.Size = new System.Drawing.Size(109, 17);
            this.ckbOK.TabIndex = 12;
            this.ckbOK.Text = "Matching 200 OK";
            this.ckbOK.UseVisualStyleBackColor = true;
            // 
            // ckbOPT
            // 
            this.ckbOPT.AutoSize = true;
            this.ckbOPT.Checked = true;
            this.ckbOPT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbOPT.Location = new System.Drawing.Point(12, 50);
            this.ckbOPT.Name = "ckbOPT";
            this.ckbOPT.Size = new System.Drawing.Size(62, 17);
            this.ckbOPT.TabIndex = 13;
            this.ckbOPT.Text = "Options";
            this.ckbOPT.UseVisualStyleBackColor = true;
            // 
            // ckbNOT
            // 
            this.ckbNOT.AutoSize = true;
            this.ckbNOT.Checked = true;
            this.ckbNOT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNOT.Location = new System.Drawing.Point(91, 50);
            this.ckbNOT.Name = "ckbNOT";
            this.ckbNOT.Size = new System.Drawing.Size(53, 17);
            this.ckbNOT.TabIndex = 14;
            this.ckbNOT.Text = "Notify";
            this.ckbNOT.UseVisualStyleBackColor = true;
            // 
            // lbRTP
            // 
            this.lbRTP.AutoSize = true;
            this.lbRTP.Enabled = false;
            this.lbRTP.Location = new System.Drawing.Point(505, 29);
            this.lbRTP.Name = "lbRTP";
            this.lbRTP.Size = new System.Drawing.Size(29, 13);
            this.lbRTP.TabIndex = 16;
            this.lbRTP.Text = "RTP";
            // 
            // ndMSBR
            // 
            this.ndMSBR.Enabled = false;
            this.ndMSBR.Location = new System.Drawing.Point(491, 48);
            this.ndMSBR.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ndMSBR.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndMSBR.Name = "ndMSBR";
            this.ndMSBR.Size = new System.Drawing.Size(65, 20);
            this.ndMSBR.TabIndex = 15;
            this.ndMSBR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndMSBR.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(372, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Packet Interval (ms)";
            // 
            // lbFilename
            // 
            this.lbFilename.AutoSize = true;
            this.lbFilename.Location = new System.Drawing.Point(12, 555);
            this.lbFilename.Name = "lbFilename";
            this.lbFilename.Size = new System.Drawing.Size(0, 13);
            this.lbFilename.TabIndex = 18;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 609);
            this.Controls.Add(this.lbFilename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbRTP);
            this.Controls.Add(this.ndMSBR);
            this.Controls.Add(this.ckbNOT);
            this.Controls.Add(this.ckbOPT);
            this.Controls.Add(this.ckbOK);
            this.Controls.Add(this.ckbREG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckbRTP);
            this.Controls.Add(this.lbMSBS);
            this.Controls.Add(this.ndMSBetweenSIP);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnHalfSecondFix);
            this.Controls.Add(this.lbPackets);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.dgvDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharkEdit";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMSBetweenSIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndMSBR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDisplay;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Label lbPackets;
        private System.Windows.Forms.Button btnHalfSecondFix;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ProgressBar pbLoading;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColPacketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColSIP;
        private System.Windows.Forms.NumericUpDown ndMSBetweenSIP;
        private System.Windows.Forms.Label lbMSBS;
        private System.Windows.Forms.CheckBox ckbRTP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbREG;
        private System.Windows.Forms.CheckBox ckbOK;
        private System.Windows.Forms.CheckBox ckbOPT;
        private System.Windows.Forms.CheckBox ckbNOT;
        private System.Windows.Forms.Label lbRTP;
        private System.Windows.Forms.NumericUpDown ndMSBR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbFilename;
    }
}


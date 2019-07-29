namespace SharkEdit
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
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.lbPackets = new System.Windows.Forms.Label();
            this.btnHalfSecondFix = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.lbTotalTime = new System.Windows.Forms.Label();
            this.dgvDisplayColPacketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColSIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDisplay
            // 
            this.dgvDisplay.AllowUserToAddRows = false;
            this.dgvDisplay.AllowUserToDeleteRows = false;
            this.dgvDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvDisplayColPacketCount,
            this.dgvDisplayColTimeStamp,
            this.dgvDisplayColLength,
            this.dgvDisplayColSIP});
            this.dgvDisplay.Location = new System.Drawing.Point(12, 41);
            this.dgvDisplay.Name = "dgvDisplay";
            this.dgvDisplay.ReadOnly = true;
            this.dgvDisplay.RowHeadersVisible = false;
            this.dgvDisplay.Size = new System.Drawing.Size(489, 397);
            this.dgvDisplay.TabIndex = 0;
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 12);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // lbPackets
            // 
            this.lbPackets.AutoSize = true;
            this.lbPackets.Location = new System.Drawing.Point(389, 449);
            this.lbPackets.Name = "lbPackets";
            this.lbPackets.Size = new System.Drawing.Size(58, 13);
            this.lbPackets.TabIndex = 2;
            this.lbPackets.Text = "Packets: 0";
            // 
            // btnHalfSecondFix
            // 
            this.btnHalfSecondFix.Location = new System.Drawing.Point(406, 12);
            this.btnHalfSecondFix.Name = "btnHalfSecondFix";
            this.btnHalfSecondFix.Size = new System.Drawing.Size(95, 23);
            this.btnHalfSecondFix.TabIndex = 3;
            this.btnHalfSecondFix.Text = "0.5 second fix";
            this.btnHalfSecondFix.UseVisualStyleBackColor = true;
            this.btnHalfSecondFix.Click += new System.EventHandler(this.btnHalfSecondFix_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 444);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // pbLoading
            // 
            this.pbLoading.Location = new System.Drawing.Point(113, 444);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(270, 23);
            this.pbLoading.TabIndex = 5;
            // 
            // lbTotalTime
            // 
            this.lbTotalTime.AutoSize = true;
            this.lbTotalTime.Location = new System.Drawing.Point(178, 479);
            this.lbTotalTime.Name = "lbTotalTime";
            this.lbTotalTime.Size = new System.Drawing.Size(132, 13);
            this.lbTotalTime.TabIndex = 6;
            this.lbTotalTime.Text = "Total Time: 00 Min 00 Sec";
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
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 501);
            this.Controls.Add(this.lbTotalTime);
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
        private System.Windows.Forms.Label lbTotalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColPacketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColSIP;
    }
}


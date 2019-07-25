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
            this.dgvDisplay = new System.Windows.Forms.DataGridView();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.lbPackets = new System.Windows.Forms.Label();
            this.dgvDisplayColPacketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDisplayColDelay = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvDisplayColInterval,
            this.dgvDisplayColDelay});
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
            this.lbPackets.Location = new System.Drawing.Point(390, 444);
            this.lbPackets.Name = "lbPackets";
            this.lbPackets.Size = new System.Drawing.Size(58, 13);
            this.lbPackets.TabIndex = 2;
            this.lbPackets.Text = "Packets: 0";
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
            // 
            // dgvDisplayColLength
            // 
            this.dgvDisplayColLength.HeaderText = "Length";
            this.dgvDisplayColLength.Name = "dgvDisplayColLength";
            this.dgvDisplayColLength.ReadOnly = true;
            this.dgvDisplayColLength.Width = 55;
            // 
            // dgvDisplayColInterval
            // 
            this.dgvDisplayColInterval.HeaderText = "Interval";
            this.dgvDisplayColInterval.Name = "dgvDisplayColInterval";
            this.dgvDisplayColInterval.ReadOnly = true;
            // 
            // dgvDisplayColDelay
            // 
            this.dgvDisplayColDelay.HeaderText = "Delay After Packet";
            this.dgvDisplayColDelay.Name = "dgvDisplayColDelay";
            this.dgvDisplayColDelay.ReadOnly = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 466);
            this.Controls.Add(this.lbPackets);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.dgvDisplay);
            this.Name = "FrmMain";
            this.Text = "SharkEdit";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDisplay;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Label lbPackets;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColPacketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDisplayColDelay;
    }
}


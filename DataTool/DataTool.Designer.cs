namespace DataTool
{
    partial class DataTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTool));
            this.starttime = new System.Windows.Forms.DateTimePicker();
            this.endtime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtsn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnsearch = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.comboBoxExportData = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbStationName = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.dataGridBiData = new System.Windows.Forms.DataGridView();
            this.dataGridBiSummary = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBiData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBiSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // starttime
            // 
            this.starttime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.starttime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.starttime.Location = new System.Drawing.Point(159, 56);
            this.starttime.Name = "starttime";
            this.starttime.Size = new System.Drawing.Size(167, 31);
            this.starttime.TabIndex = 15;
            // 
            // endtime
            // 
            this.endtime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.endtime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endtime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endtime.Location = new System.Drawing.Point(527, 56);
            this.endtime.Name = "endtime";
            this.endtime.Size = new System.Drawing.Size(162, 31);
            this.endtime.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.CausesValidation = false;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(344, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "End Time:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.CausesValidation = false;
            this.label3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Start Time:";
            // 
            // txtsn
            // 
            this.txtsn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtsn.Location = new System.Drawing.Point(159, 14);
            this.txtsn.Multiline = true;
            this.txtsn.Name = "txtsn";
            this.txtsn.Size = new System.Drawing.Size(167, 27);
            this.txtsn.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.CausesValidation = false;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Number:";
            // 
            // btnsearch
            // 
            this.btnsearch.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.Location = new System.Drawing.Point(856, 57);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(108, 32);
            this.btnsearch.TabIndex = 0;
            this.btnsearch.Text = "Search";
            this.btnsearch.UseVisualStyleBackColor = true;
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1396, 880);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.comboBoxExportData);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbStationName);
            this.panel1.Controls.Add(this.btnsearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.endtime);
            this.panel1.Controls.Add(this.starttime);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtsn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1390, 141);
            this.panel1.TabIndex = 8;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(359, 95);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(108, 32);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // comboBoxExportData
            // 
            this.comboBoxExportData.FormattingEnabled = true;
            this.comboBoxExportData.Location = new System.Drawing.Point(159, 99);
            this.comboBoxExportData.Name = "comboBoxExportData";
            this.comboBoxExportData.Size = new System.Drawing.Size(167, 21);
            this.comboBoxExportData.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.CausesValidation = false;
            this.label5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "Export Data:";
            // 
            // tbStationName
            // 
            this.tbStationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStationName.Location = new System.Drawing.Point(719, 58);
            this.tbStationName.Name = "tbStationName";
            this.tbStationName.Size = new System.Drawing.Size(123, 31);
            this.tbStationName.TabIndex = 0;
            this.tbStationName.Text = "localhost";
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(0, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 880);
            this.splitter2.TabIndex = 6;
            this.splitter2.TabStop = false;
            // 
            // dataGridBiData
            // 
            this.dataGridBiData.AllowUserToAddRows = false;
            this.dataGridBiData.AllowUserToDeleteRows = false;
            this.dataGridBiData.AllowUserToResizeRows = false;
            this.dataGridBiData.BackgroundColor = System.Drawing.Color.Azure;
            this.dataGridBiData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBiData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridBiData.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridBiData.Location = new System.Drawing.Point(0, 0);
            this.dataGridBiData.Name = "dataGridBiData";
            this.dataGridBiData.RowHeadersVisible = false;
            this.dataGridBiData.Size = new System.Drawing.Size(1390, 467);
            this.dataGridBiData.TabIndex = 0;
            // 
            // dataGridBiSummary
            // 
            this.dataGridBiSummary.AllowUserToAddRows = false;
            this.dataGridBiSummary.AllowUserToDeleteRows = false;
            this.dataGridBiSummary.AllowUserToResizeRows = false;
            this.dataGridBiSummary.BackgroundColor = System.Drawing.Color.Azure;
            this.dataGridBiSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBiSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridBiSummary.Location = new System.Drawing.Point(0, 0);
            this.dataGridBiSummary.Name = "dataGridBiSummary";
            this.dataGridBiSummary.RowHeadersVisible = false;
            this.dataGridBiSummary.Size = new System.Drawing.Size(1390, 256);
            this.dataGridBiSummary.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 150);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridBiSummary);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridBiData);
            this.splitContainer1.Size = new System.Drawing.Size(1390, 727);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 7;
            // 
            // DataTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 880);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Tool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchUI_FormClosed);
            this.Load += new System.EventHandler(this.DataTool_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBiData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBiSummary)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnsearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtsn;
        private System.Windows.Forms.DateTimePicker starttime;
        private System.Windows.Forms.DateTimePicker endtime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbStationName;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ComboBox comboBoxExportData;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridBiSummary;
        private System.Windows.Forms.DataGridView dataGridBiData;
    }
}
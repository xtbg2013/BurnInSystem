namespace BurnInUI.Forms
{
    partial class ParameterShowUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.gpbchart = new System.Windows.Forms.GroupBox();
            this.Chartpara = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pgProduct = new System.Windows.Forms.PropertyGrid();
            this.btnFetch = new System.Windows.Forms.Button();
            this.dgvParamShowList = new System.Windows.Forms.DataGridView();
            this.IsShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtsn = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbShowSpec = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ckbSelect = new System.Windows.Forms.CheckBox();
            this.gpbchart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Chartpara)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamShowList)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbchart
            // 
            this.gpbchart.Controls.Add(this.Chartpara);
            this.gpbchart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpbchart.Location = new System.Drawing.Point(3, 303);
            this.gpbchart.Name = "gpbchart";
            this.gpbchart.Size = new System.Drawing.Size(494, 294);
            this.gpbchart.TabIndex = 3;
            this.gpbchart.TabStop = false;
            this.gpbchart.Text = "Parameter Values Trend";
            // 
            // Chartpara
            // 
            chartArea2.AlignmentOrientation = ((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations)((System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Vertical | System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal)));
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.Name = "ChartArea1";
            this.Chartpara.ChartAreas.Add(chartArea2);
            this.Chartpara.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.Chartpara.Legends.Add(legend2);
            this.Chartpara.Location = new System.Drawing.Point(3, 16);
            this.Chartpara.Name = "Chartpara";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.MarkerBorderColor = System.Drawing.Color.Blue;
            series2.MarkerBorderWidth = 3;
            series2.MarkerColor = System.Drawing.Color.Blue;
            series2.MarkerStep = 2;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Triangle;
            series2.Name = "Values";
            series2.YValuesPerPoint = 2;
            this.Chartpara.Series.Add(series2);
            this.Chartpara.Size = new System.Drawing.Size(488, 275);
            this.Chartpara.TabIndex = 3;
            this.Chartpara.Text = "Chartpara";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.pgProduct);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 248);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Property";
            // 
            // pgProduct
            // 
            this.pgProduct.AllowDrop = true;
            this.pgProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgProduct.HelpVisible = false;
            this.pgProduct.Location = new System.Drawing.Point(3, 12);
            this.pgProduct.Name = "pgProduct";
            this.pgProduct.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgProduct.Size = new System.Drawing.Size(256, 215);
            this.pgProduct.TabIndex = 0;
            this.pgProduct.ToolbarVisible = false;
            // 
            // btnFetch
            // 
            this.btnFetch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFetch.Location = new System.Drawing.Point(121, 0);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(93, 34);
            this.btnFetch.TabIndex = 5;
            this.btnFetch.Text = "Fetch";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // dgvParamShowList
            // 
            this.dgvParamShowList.AllowUserToAddRows = false;
            this.dgvParamShowList.AllowUserToDeleteRows = false;
            this.dgvParamShowList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParamShowList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsShow,
            this.Parameter});
            this.dgvParamShowList.Location = new System.Drawing.Point(274, 3);
            this.dgvParamShowList.Name = "dgvParamShowList";
            this.dgvParamShowList.RowHeadersVisible = false;
            this.dgvParamShowList.Size = new System.Drawing.Size(217, 248);
            this.dgvParamShowList.TabIndex = 4;
            this.dgvParamShowList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParamShowList_CellValueChanged);
            this.dgvParamShowList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvParamShowList_CurrentCellDirtyStateChanged);
            // 
            // IsShow
            // 
            this.IsShow.HeaderText = "";
            this.IsShow.Name = "IsShow";
            this.IsShow.Width = 20;
            // 
            // Parameter
            // 
            this.Parameter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Parameter.HeaderText = "Available Parameter";
            this.Parameter.Name = "Parameter";
            this.Parameter.ReadOnly = true;
            // 
            // txtsn
            // 
            this.txtsn.Location = new System.Drawing.Point(145, 7);
            this.txtsn.Multiline = true;
            this.txtsn.Name = "txtsn";
            this.txtsn.ReadOnly = true;
            this.txtsn.Size = new System.Drawing.Size(94, 23);
            this.txtsn.TabIndex = 4;
            this.txtsn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtsn.Visible = false;
            this.txtsn.TextChanged += new System.EventHandler(this.txtsn_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gpbchart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 600);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvParamShowList, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(494, 294);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbShowSpec);
            this.panel1.Controls.Add(this.txtsn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 257);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 34);
            this.panel1.TabIndex = 7;
            // 
            // cbShowSpec
            // 
            this.cbShowSpec.AutoSize = true;
            this.cbShowSpec.Location = new System.Drawing.Point(3, 9);
            this.cbShowSpec.Name = "cbShowSpec";
            this.cbShowSpec.Size = new System.Drawing.Size(117, 17);
            this.cbShowSpec.TabIndex = 5;
            this.cbShowSpec.Text = "Show Specification";
            this.cbShowSpec.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.ckbSelect);
            this.panel2.Controls.Add(this.btnFetch);
            this.panel2.Location = new System.Drawing.Point(274, 257);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 34);
            this.panel2.TabIndex = 8;
            // 
            // ckbSelect
            // 
            this.ckbSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ckbSelect.AutoSize = true;
            this.ckbSelect.Location = new System.Drawing.Point(17, 6);
            this.ckbSelect.Name = "ckbSelect";
            this.ckbSelect.Size = new System.Drawing.Size(67, 17);
            this.ckbSelect.TabIndex = 6;
            this.ckbSelect.Text = "SelectAll";
            this.ckbSelect.UseVisualStyleBackColor = true;
            this.ckbSelect.CheckedChanged += new System.EventHandler(this.ckbSelect_CheckedChanged);
            // 
            // ParameterShowUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ParameterShowUC";
            this.Size = new System.Drawing.Size(500, 600);
            this.gpbchart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Chartpara)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParamShowList)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtsn;
        private System.Windows.Forms.GroupBox gpbchart;
        private System.Windows.Forms.DataVisualization.Charting.Chart Chartpara;
        private System.Windows.Forms.DataGridView dgvParamShowList;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PropertyGrid pgProduct;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox ckbSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.CheckBox cbShowSpec;
    }
}

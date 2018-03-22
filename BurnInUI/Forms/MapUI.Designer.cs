namespace BurnInUI.Forms
{
    partial class MapUi
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
            this.labelTsName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbBoardName = new System.Windows.Forms.TextBox();
            this.tbBoardFloor = new System.Windows.Forms.TextBox();
            this.tbBoardNumber = new System.Windows.Forms.TextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.dgMapping = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_mapName = new System.Windows.Forms.ComboBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.textBoxSeatRows = new System.Windows.Forms.TextBox();
            this.textBoxBoardCols = new System.Windows.Forms.TextBox();
            this.textBoxBoardRows = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSeatCols = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTsName
            // 
            this.labelTsName.AutoSize = true;
            this.labelTsName.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTsName.Location = new System.Drawing.Point(34, 243);
            this.labelTsName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTsName.Name = "labelTsName";
            this.labelTsName.Size = new System.Drawing.Size(121, 25);
            this.labelTsName.TabIndex = 1;
            this.labelTsName.Text = "Board Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 320);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Board Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 281);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Board Floor:";
            // 
            // tbBoardName
            // 
            this.tbBoardName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBoardName.Location = new System.Drawing.Point(178, 245);
            this.tbBoardName.Margin = new System.Windows.Forms.Padding(2);
            this.tbBoardName.Name = "tbBoardName";
            this.tbBoardName.Size = new System.Drawing.Size(57, 26);
            this.tbBoardName.TabIndex = 5;
            // 
            // tbBoardFloor
            // 
            this.tbBoardFloor.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBoardFloor.Location = new System.Drawing.Point(178, 283);
            this.tbBoardFloor.Margin = new System.Windows.Forms.Padding(2);
            this.tbBoardFloor.Name = "tbBoardFloor";
            this.tbBoardFloor.Size = new System.Drawing.Size(57, 26);
            this.tbBoardFloor.TabIndex = 6;
            // 
            // tbBoardNumber
            // 
            this.tbBoardNumber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBoardNumber.Location = new System.Drawing.Point(178, 322);
            this.tbBoardNumber.Margin = new System.Windows.Forms.Padding(2);
            this.tbBoardNumber.Name = "tbBoardNumber";
            this.tbBoardNumber.Size = new System.Drawing.Size(57, 26);
            this.tbBoardNumber.TabIndex = 7;
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(146, 492);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(89, 30);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dgMapping
            // 
            this.dgMapping.AllowUserToAddRows = false;
            this.dgMapping.AllowUserToDeleteRows = false;
            this.dgMapping.AllowUserToResizeRows = false;
            this.dgMapping.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMapping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgMapping.Location = new System.Drawing.Point(302, 35);
            this.dgMapping.Margin = new System.Windows.Forms.Padding(2);
            this.dgMapping.Name = "dgMapping";
            this.dgMapping.ReadOnly = true;
            this.dgMapping.RowHeadersVisible = false;
            this.dgMapping.RowTemplate.Height = 24;
            this.dgMapping.Size = new System.Drawing.Size(354, 555);
            this.dgMapping.TabIndex = 13;
            this.dgMapping.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgMapping_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Map Scheme:";
            // 
            // comboBox_mapName
            // 
            this.comboBox_mapName.FormattingEnabled = true;
            this.comboBox_mapName.Location = new System.Drawing.Point(157, 35);
            this.comboBox_mapName.Name = "comboBox_mapName";
            this.comboBox_mapName.Size = new System.Drawing.Size(121, 21);
            this.comboBox_mapName.TabIndex = 16;
            this.comboBox_mapName.SelectedIndexChanged += new System.EventHandler(this.comboBox_mapName_SelectedIndexChanged);
            this.comboBox_mapName.TextUpdate += new System.EventHandler(this.comboBox_mapName_TextUpdate);
            // 
            // btnInsert
            // 
            this.btnInsert.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsert.Location = new System.Drawing.Point(146, 390);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(2);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(89, 30);
            this.btnInsert.TabIndex = 17;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(27, 438);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(89, 30);
            this.btnLoad.TabIndex = 18;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(27, 390);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(89, 30);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(146, 438);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // textBoxSeatRows
            // 
            this.textBoxSeatRows.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSeatRows.Location = new System.Drawing.Point(178, 156);
            this.textBoxSeatRows.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSeatRows.Name = "textBoxSeatRows";
            this.textBoxSeatRows.Size = new System.Drawing.Size(57, 26);
            this.textBoxSeatRows.TabIndex = 25;
            // 
            // textBoxBoardCols
            // 
            this.textBoxBoardCols.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBoardCols.Location = new System.Drawing.Point(178, 117);
            this.textBoxBoardCols.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBoardCols.Name = "textBoxBoardCols";
            this.textBoxBoardCols.Size = new System.Drawing.Size(57, 26);
            this.textBoxBoardCols.TabIndex = 24;
            // 
            // textBoxBoardRows
            // 
            this.textBoxBoardRows.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBoardRows.Location = new System.Drawing.Point(178, 79);
            this.textBoxBoardRows.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBoardRows.Name = "textBoxBoardRows";
            this.textBoxBoardRows.Size = new System.Drawing.Size(57, 26);
            this.textBoxBoardRows.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(33, 115);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 25);
            this.label4.TabIndex = 22;
            this.label4.Text = "Board Cols";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(33, 154);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 25);
            this.label5.TabIndex = 21;
            this.label5.Text = "Seat Rows";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 77);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 25);
            this.label6.TabIndex = 20;
            this.label6.Text = "Board Rows:";
            // 
            // textBoxSeatCols
            // 
            this.textBoxSeatCols.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSeatCols.Location = new System.Drawing.Point(178, 199);
            this.textBoxSeatCols.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSeatCols.Name = "textBoxSeatCols";
            this.textBoxSeatCols.Size = new System.Drawing.Size(57, 26);
            this.textBoxSeatCols.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(33, 197);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 25);
            this.label7.TabIndex = 27;
            this.label7.Text = "Seat Cols";
            // 
            // MapUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(707, 648);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxSeatCols);
            this.Controls.Add(this.textBoxSeatRows);
            this.Controls.Add(this.textBoxBoardCols);
            this.Controls.Add(this.textBoxBoardRows);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.comboBox_mapName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgMapping);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbBoardNumber);
            this.Controls.Add(this.tbBoardFloor);
            this.Controls.Add(this.tbBoardName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTsName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MapUi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAPPING";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapUI_FormClosed);
            this.Load += new System.EventHandler(this.MapUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgMapping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTsName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbBoardName;
        private System.Windows.Forms.TextBox tbBoardFloor;
        private System.Windows.Forms.TextBox tbBoardNumber;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DataGridView dgMapping;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_mapName;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox textBoxSeatRows;
        private System.Windows.Forms.TextBox textBoxBoardCols;
        private System.Windows.Forms.TextBox textBoxBoardRows;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSeatCols;
        private System.Windows.Forms.Label label7;
    }
}
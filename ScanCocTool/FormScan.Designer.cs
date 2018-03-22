namespace ScanCocTool
{
    partial class FormScan
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
            this.dataGridViewBind = new System.Windows.Forms.DataGridView();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.dataGridViewBh1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewBh2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBh1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBh2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBind
            // 
            this.dataGridViewBind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBind.Location = new System.Drawing.Point(12, 38);
            this.dataGridViewBind.Name = "dataGridViewBind";
            this.dataGridViewBind.Size = new System.Drawing.Size(216, 724);
            this.dataGridViewBind.TabIndex = 0;
            this.dataGridViewBind.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewBind_RowPostPaint);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(884, 116);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 44);
            this.buttonLoadFile.TabIndex = 1;
            this.buttonLoadFile.TabStop = false;
            this.buttonLoadFile.Text = "LoadFile";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Visible = false;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(884, 38);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(75, 44);
            this.buttonSubmit.TabIndex = 2;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // dataGridViewBh1
            // 
            this.dataGridViewBh1.AllowUserToAddRows = false;
            this.dataGridViewBh1.AllowUserToDeleteRows = false;
            this.dataGridViewBh1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBh1.Location = new System.Drawing.Point(243, 38);
            this.dataGridViewBh1.Name = "dataGridViewBh1";
            this.dataGridViewBh1.Size = new System.Drawing.Size(597, 349);
            this.dataGridViewBh1.TabIndex = 3;
            // 
            // dataGridViewBh2
            // 
            this.dataGridViewBh2.AllowUserToAddRows = false;
            this.dataGridViewBh2.AllowUserToDeleteRows = false;
            this.dataGridViewBh2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBh2.Location = new System.Drawing.Point(243, 413);
            this.dataGridViewBh2.Name = "dataGridViewBh2";
            this.dataGridViewBh2.Size = new System.Drawing.Size(596, 349);
            this.dataGridViewBh2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "SN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(240, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "BH1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(240, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "BH2 OR BH1&&BH2";
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 774);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewBh2);
            this.Controls.Add(this.dataGridViewBh1);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.buttonLoadFile);
            this.Controls.Add(this.dataGridViewBind);
            this.Name = "FormScan";
            this.Text = "CFP8  SCAN";
            this.Load += new System.EventHandler(this.FormScan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBh1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBh2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewBind;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.DataGridView dataGridViewBh1;
        private System.Windows.Forms.DataGridView dataGridViewBh2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}


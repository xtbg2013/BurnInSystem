namespace BurnInUI.Forms
{
    partial class DatabaseControl
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
            this.btnCreateLocalDb = new System.Windows.Forms.Button();
            this.btnCreateRemoteDb = new System.Windows.Forms.Button();
            this.checkBoxUpload = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCreateLocalDb
            // 
            this.btnCreateLocalDb.Location = new System.Drawing.Point(45, 72);
            this.btnCreateLocalDb.Name = "btnCreateLocalDb";
            this.btnCreateLocalDb.Size = new System.Drawing.Size(110, 36);
            this.btnCreateLocalDb.TabIndex = 0;
            this.btnCreateLocalDb.Text = "Create locol  database";
            this.btnCreateLocalDb.UseVisualStyleBackColor = true;
            this.btnCreateLocalDb.Click += new System.EventHandler(this.btnCreateLocalDb_Click);
            // 
            // btnCreateRemoteDb
            // 
            this.btnCreateRemoteDb.Location = new System.Drawing.Point(45, 123);
            this.btnCreateRemoteDb.Name = "btnCreateRemoteDb";
            this.btnCreateRemoteDb.Size = new System.Drawing.Size(110, 38);
            this.btnCreateRemoteDb.TabIndex = 1;
            this.btnCreateRemoteDb.Text = "Create remote  database";
            this.btnCreateRemoteDb.UseVisualStyleBackColor = true;
            this.btnCreateRemoteDb.Click += new System.EventHandler(this.btnCreateRemoteDb_Click);
            // 
            // checkBoxUpload
            // 
            this.checkBoxUpload.AutoSize = true;
            this.checkBoxUpload.Location = new System.Drawing.Point(45, 38);
            this.checkBoxUpload.Name = "checkBoxUpload";
            this.checkBoxUpload.Size = new System.Drawing.Size(95, 17);
            this.checkBoxUpload.TabIndex = 2;
            this.checkBoxUpload.Text = "Upload BiData";
            this.checkBoxUpload.UseVisualStyleBackColor = true;
            this.checkBoxUpload.CheckStateChanged += new System.EventHandler(this.checkBoxUpload_CheckStateChanged);
            // 
            // DatabaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 209);
            this.Controls.Add(this.checkBoxUpload);
            this.Controls.Add(this.btnCreateRemoteDb);
            this.Controls.Add(this.btnCreateLocalDb);
            this.Name = "DatabaseControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseControl";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DatabaseControl_FormClosed);
            this.Load += new System.EventHandler(this.DatabaseControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateLocalDb;
        private System.Windows.Forms.Button btnCreateRemoteDb;
        private System.Windows.Forms.CheckBox checkBoxUpload;
    }
}
namespace BurnInUI.Forms
{
    partial class BoardUc
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
            this.components = new System.ComponentModel.Container();
            this.cmsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsRightClick
            // 
            this.cmsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableToolStripMenuItem,
            this.enableToolStripMenuItem,
            this.recoverToolStripMenuItem});
            this.cmsRightClick.Name = "cmsRightClick";
            this.cmsRightClick.Size = new System.Drawing.Size(117, 70);
            this.cmsRightClick.Opened += new System.EventHandler(this.cmsRightClick_Opened);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.DisableToolStripMenuItem_Click);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.enableToolStripMenuItem.Text = "Enable";
            // 
            // recoverToolStripMenuItem
            // 
            this.recoverToolStripMenuItem.Name = "recoverToolStripMenuItem";
            this.recoverToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.recoverToolStripMenuItem.Text = "Recover";
            this.recoverToolStripMenuItem.Click += new System.EventHandler(this.recoverToolStripMenuItem_Click);
            // 
            // BoardUc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BoardUc";
            this.Size = new System.Drawing.Size(375, 406);
            this.Load += new System.EventHandler(this.BoardUC_Load);
            this.cmsRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsRightClick;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recoverToolStripMenuItem;
    }
}

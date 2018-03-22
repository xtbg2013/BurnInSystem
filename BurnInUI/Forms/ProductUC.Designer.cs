namespace BurnInUI.Forms
{
    partial class ProductUc
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
            this.cmsUnitDisable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsUnitDisable.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsUnitDisable
            // 
            this.cmsUnitDisable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableToolStripMenuItem});
            this.cmsUnitDisable.Name = "cmsUnitDisable";
            this.cmsUnitDisable.Size = new System.Drawing.Size(118, 26);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.disableToolStripMenuItem.Text = "Remove";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // ProductUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ProductUc";
            this.Size = new System.Drawing.Size(375, 406);
            this.Load += new System.EventHandler(this.ProductUC_Load);
            this.cmsUnitDisable.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsUnitDisable;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
    }
}

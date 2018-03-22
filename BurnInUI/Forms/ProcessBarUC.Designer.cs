namespace BurnInUI.Forms
{
    partial class ProcessBarUC
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
            this.tlpProcessBar = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tlpProcessBar
            // 
            this.tlpProcessBar.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tlpProcessBar.ColumnCount = 6;
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.55012F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.55012F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.55012F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.55012F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.55012F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.24942F));
            this.tlpProcessBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProcessBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpProcessBar.Location = new System.Drawing.Point(0, 0);
            this.tlpProcessBar.Margin = new System.Windows.Forms.Padding(2);
            this.tlpProcessBar.Name = "tlpProcessBar";
            this.tlpProcessBar.RowCount = 1;
            this.tlpProcessBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProcessBar.Size = new System.Drawing.Size(440, 45);
            this.tlpProcessBar.TabIndex = 0;
            // 
            // ProcessBarUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlpProcessBar);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ProcessBarUC";
            this.Size = new System.Drawing.Size(440, 45);
            this.Load += new System.EventHandler(this.ProcessBarUC_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProcessBar;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BurnInUI.Forms
{
    public partial class InitForm : Form
    {

        private static InitForm _inst;
        private Bitmap _bitmap;
        public static InitForm Instance()
        {
            return  _inst ?? (_inst = new InitForm());
        }
        protected InitForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            const string info = @"Start initialize,please wait ......";
            _bitmap = new Bitmap(Properties.Resources.InitImage);
            ClientSize = _bitmap.Size;
            using (Font font = new Font("Consoles",30, FontStyle.Bold))
            {
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.DrawString(info, Font, Brushes.White, 300, 100);
                }
            }
            BackgroundImage = _bitmap;
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
             

        }

        private void InitForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}

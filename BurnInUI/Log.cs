using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;
namespace BurnInUI
{
    public class TextBoxAppender:AppenderSkeleton
    {
        private TextBoxBase _textBox;
        public string FormName { get; set; }
        public string TableLayoutPanelName { get; set; }
        public string PanelName { get; set; }
        public string TabControlName { get; set; }
        public string TabPageName { get; set; }
        public string TextBoxName { get; set; }

        public TextBoxAppender()
        {
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var logInfo = string.Empty;
            if (_textBox == null)
            {
                if (string.IsNullOrEmpty(FormName) || string.IsNullOrEmpty(TextBoxName))
                    return;
                Form form = Application.OpenForms[FormName];
                if (form == null) return;
                TableLayoutPanel layoutPanel = form.Controls[TableLayoutPanelName] as TableLayoutPanel;
                if (layoutPanel == null) return;
                Panel panel = layoutPanel.Controls[PanelName] as Panel;
                if (panel == null) return;
                TabControl tabControl = panel.Controls[TabControlName] as TabControl;
                if (tabControl == null) return;
                TabPage tabPage = tabControl.Controls[TabPageName] as TabPage;
                if (tabPage == null) return;
                _textBox = tabPage.Controls[TextBoxName] as TextBox;
                if (_textBox == null)return;
                form.FormClosed += (s, e) => _textBox = null;
            }

            _textBox.BeginInvoke((MethodInvoker)delegate
            {
                _textBox.AppendText(RenderLoggingEvent(loggingEvent));
                _textBox.Focus();
                _textBox.Select(_textBox.TextLength, 0);
                _textBox.ScrollToCaret();
            });
           
        }
    }
   
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProgressODoom;

namespace BurnInUI.Forms
{
    public partial class ProcessBarUC : UserControl
    {
        private ProgressBarEx[] boardList;
        private const int BOARDCOLUMCOUNT = 6;
        private Dictionary<int, PlainBackgroundPainter> bgPainterDict;
        private Dictionary<int, string> labelTextDict;

        private Label[] lblList;

        public ProcessBarUC()
        {
            InitBoardValuable();
            InitializeComponent();
        }
        private void InitBoardValuable()
        {
            boardList = new ProgressBarEx[BOARDCOLUMCOUNT];
            lblList = new Label[BOARDCOLUMCOUNT];
            CreateBackgroundPainterDict();
            CreateLableTextDict();
        }
        private void AddProgressBar()
        {
            this.tlpProcessBar.SuspendLayout();
            PaintFirstRowUI();
            this.tlpProcessBar.ResumeLayout(false);
        }

        private void ProcessBarUC_Load(object sender, EventArgs e)
        {
            this.AddProgressBar();
        }

        private void CreateBackgroundPainterDict()
        {
            bgPainterDict = new Dictionary<int, PlainBackgroundPainter>();
            string[] state = new string[] {"EMPTY","READY","BURNIN","PAUSE","PASS","FAIL"};
            for(int i=0;i<state.Length;i++)
            {
                bgPainterDict.Add(i,PainterFactory.GetProductStatePainter(state[i]));
            }
        }

        private void CreateLableTextDict()
        {
            labelTextDict = new Dictionary<int, string>();
            string[] state = new string[] { "EMPTY", "READY", "BURNIN", "PAUSE", "PASS", "FAIL" };
            for (int i = 0; i < state.Length; i++)
            {
                labelTextDict.Add(i, state[i]);
            }
        }
        private void PaintFirstRowUI()
        {
            for (int i = 0; i < BOARDCOLUMCOUNT; i++)
            {
                boardList[i] = new ProgressBarEx();
                boardList[i].BackgroundPainter = bgPainterDict[i];  //differ
                boardList[i].BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");
                boardList[i].Dock = System.Windows.Forms.DockStyle.Fill;
                boardList[i].Location = new System.Drawing.Point(4 + BOARDCOLUMCOUNT * 54, 11);// + (i / BOARDCOLUMCOUNT) * 43);
                boardList[i].MarqueePercentage = 25;
                boardList[i].MarqueeSpeed = 30;
                boardList[i].MarqueeStep = 1;
                boardList[i].Maximum = 100;
                boardList[i].Minimum = 0;
                boardList[i].ProgressPadding = 0;
                boardList[i].ProgressType = ProgressODoom.ProgressType.Smooth;
                boardList[i].ShowPercentage = false;
                boardList[i].Name = "pbeProcessBarUC" + (BOARDCOLUMCOUNT + i).ToString();
                boardList[i].Text = labelTextDict[i];
                boardList[i].ForeColor = Color.Black;
                boardList[i].Font = new Font(FontFamily.GenericSansSerif,8);
                this.tlpProcessBar.Controls.Add(boardList[i],i,0);
            }
        }
    }
}

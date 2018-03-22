using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BILib;
using BIModel;
using BIModel.Interface;
using ProgressODoom;

namespace BurnInUI.Forms
{
    public partial class ProductUc : UserControl
    {
      
        private ProgressBarEx[] _progressBarList;
        private readonly int _productpositioncount;
        private readonly int _seatRows;
        private readonly int _seatCols;
        private int ActiveSeat { get; set; }
        private string ActiveBoardName { get; set; }
        public EventHandler NotifyActiveSn { get; set; }

        private volatile bool redraw = false;
        private Timer redrawTimer;
        private readonly IBiModel _model;
        private void ProductUC_Load(object sender, EventArgs e)
        {
            var tbLayoutPanel = new TableLayoutPanel();
            tbLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            tbLayoutPanel.Name = "tlpProduct";
            tbLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            tbLayoutPanel.ColumnCount = _seatCols;
            for (int i = 0; i < _seatCols; i++)
            {
                tbLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (float)(100.0 / _seatCols)));
            }
            tbLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tbLayoutPanel.Location = new System.Drawing.Point(0, 0);

            tbLayoutPanel.RowCount = _seatRows;
            for (int i = 0; i < _seatRows; i++)
            {
                tbLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, (float)(100.0 / _seatRows)));
            }
            tbLayoutPanel.Size = this.Parent.Size;
            tbLayoutPanel.TabIndex = 0;
            this.Controls.Add(tbLayoutPanel);

            _progressBarList = new ProgressBarEx[_productpositioncount];

            tbLayoutPanel.SuspendLayout();
            for (int i = 0; i < _productpositioncount; i++)
            {
                _progressBarList[i] = new ProgressBarEx();
                _progressBarList[i].BackgroundPainter = PainterFactory.GetProductStatePainter("EMPTY");
                _progressBarList[i].ProgressPainter = PainterFactory.GetProgressPainter("BURNIN");
                _progressBarList[i].BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");
                _progressBarList[i].Dock = System.Windows.Forms.DockStyle.Fill;
                _progressBarList[i].Location = new System.Drawing.Point(6 + (i % _seatCols) * 166, 6 + (i / _seatCols) * 124);
                _progressBarList[i].MarqueePercentage = 25;
                _progressBarList[i].MarqueeSpeed = 30;
                _progressBarList[i].MarqueeStep = 1;
                _progressBarList[i].Maximum = 100;
                _progressBarList[i].Minimum = 0;
                _progressBarList[i].Name = "pbeProduct" + i.ToString();
                _progressBarList[i].ProgressPadding = 0;
                _progressBarList[i].ProgressType = ProgressODoom.ProgressType.Smooth;
                _progressBarList[i].ShowPercentage = false;
                _progressBarList[i].Text = (i + 1).ToString() + ": ";
                _progressBarList[i].Value = 0;
                _progressBarList[i].MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProgressBarEx_MouseDown);
                _progressBarList[i].ContextMenuStrip = null;
                tbLayoutPanel.Controls.Add(_progressBarList[i]);
            }
            tbLayoutPanel.ResumeLayout(false);
        }
    

        private void ProgressBarEx_MouseDown(object sender, MouseEventArgs e)
        {
            var activeCell = (ProgressBarEx)sender;
            string content = activeCell.Text;
            string sn = content.Substring(content.IndexOf(":") + 1).Trim();
            this.ActiveSeat = int.Parse(content.Split(new char[] {':'})[0]);
            ActiveProduct(activeCell);
            if (this.NotifyActiveSn != null)
            {
                this.NotifyActiveSn(sn, null);
            }
        }
        private void ActiveProduct(ProgressBarEx bar)
        {
            foreach (ProgressBarEx productBar in this._progressBarList)
            {
                productBar.BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");
            }
            bar.BorderPainter = PainterFactory.GetPlainBoarderPainter("ACTIVE");
        }
     
    
        public override void Refresh()
        {
            this.RefreshDutsStatus();
            base.Refresh();
        }

        private void RefreshDutsStatus()
        {
            BoardState currentBoardState = this._model.GetBoardState(this.ActiveBoardName);
            if (currentBoardState != BoardState.SELECTED && currentBoardState != BoardState.UNSELECTED)
            {
                int dutCount = this._model.GetBoardSeatsCount(ActiveBoardName);
                string[] productStatus = this._model.GetProductStateOnBoard(ActiveBoardName);
                for (int i = 0; i < this._progressBarList.Length; i++)
                {
                    if (i < dutCount)
                    {
                        int seat = i + 1;
                        string sn = this._model.GetSnByPos(ActiveBoardName, seat);
                        this._progressBarList[i].Text = seat + ": " + sn;
                        this._progressBarList[i].BackgroundPainter = PainterFactory.GetProductStatePainter(productStatus[i]);
                        this._progressBarList[i].Value = (productStatus[i] == "BURNIN") ? this._model.GetProgress(sn) : 0;
                        this._progressBarList[i].ContextMenuStrip = (currentBoardState == BoardState.LOADED && productStatus[i] != "EMPTY")?this.cmsUnitDisable:null;
                    }
                    else
                    {
                        this._progressBarList[i].Text = (i + 1) + ": ";
                        this._progressBarList[i].BackgroundPainter = PainterFactory.GetProductStatePainter("EMPTY");
                        this._progressBarList[i].Value = 0;
                        this._progressBarList[i].ContextMenuStrip = null;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _progressBarList.Length; i++)
                {
                    this._progressBarList[i].Text = (i + 1).ToString() + ": ";
                    this._progressBarList[i].BackgroundPainter = PainterFactory.GetProductStatePainter("EMPTY");
                    this._progressBarList[i].Value = 0;
                }
            }
        }
        
     

        public ProductUc(IBiModel model, IDatabaseService dataService)
        {
            InitializeComponent();
            var posMapBlock =  BiModelFactory.CreateIPosMapScheme(dataService).GetDefaultPosMapBlock();
            _seatRows = posMapBlock.SeatRows;
            _seatCols = posMapBlock.SeatCols;
            _productpositioncount = _seatRows * _seatCols;
            _model = model;
            _model.ProductsUpdate += (s, e) => { redraw = true; };
            redrawTimer = new Timer() {Interval = 500};
            redrawTimer.Tick += (s, e) =>
            {
                try
                {
                    redrawTimer.Stop();
                    if (redraw == true)
                        this.BeginInvoke(new Action(Refresh));
                }
                finally
                {
                    redraw = false;
                    redrawTimer.Start();
                }
            };
            redrawTimer.Start();
        }

        public void OnBoardActive(object sender, EventArgs args)
        {
            var locateArgs = (BoardLocateEventArgs)args;
            this.ActiveBoardName = locateArgs.BoardName;
            this.redraw = true;
        }

     
        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this._model.UnitAbort(this.ActiveBoardName, this.ActiveSeat);
            });
        }
    }
}

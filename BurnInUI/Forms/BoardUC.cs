using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BILib;
using BIModel;
using BIModel.Interface;

namespace BurnInUI.Forms
{
    public partial class BoardUc : UserControl
    {
        private volatile bool _redraw;
        private BoardCell[,] _boardList;//row,col
        private BoardCell _currentClickBoard;
        private readonly IBiModel _model;
        private readonly int _boardRowNumber;
        private readonly int _boardColNumber;
        private EventHandler _activeBoardEvent;
        private EventHandler _notifyState;
        private readonly IPosMapScheme _posMapScheme;

        public BoardUc(IBiModel model, IDatabaseService dataService)
        {
            InitializeComponent();
            _model = model;
            _posMapScheme = BiModelFactory.CreateIPosMapScheme(dataService);
            _boardRowNumber = _posMapScheme.GetDefaultPosMapBlock().BoardRows;
            _boardColNumber = _posMapScheme.GetDefaultPosMapBlock().BoardCols;
            _model.BoardStateChanged += (s, e) => { _redraw = true; };
            var redrawTimer = new Timer() { Interval = 500 };
            redrawTimer.Tick += (s, e) =>
            {
                try
                {
                    redrawTimer.Stop();
                    if (_redraw == true)
                        this.BeginInvoke(new Action(Refresh));
                }
                finally
                {
                    _redraw = false;
                    redrawTimer.Start();
                }
            };
            redrawTimer.Start();
        }

        public BoardUc()
        {
        }

        public override void Refresh()
        {
            int panelrow = this._boardRowNumber;
            int panelcol = this._boardColNumber + 1;

            for (int row = panelrow - 1; row > -1; row--)
                for (int col = 0; col < panelcol; col++)
                {
                    BoardState state = this._model.GetBoardState(_boardList[row,col].Text);
                    this._boardList[row,col].BackgroundPainter = PainterFactory.GetBoardStatePainter(state.ToString());
                    if (state == BoardState.UNSELECTED)
                    {
                        this._boardList[row,col].BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");
                    }
                }
            
            if (this.NotifyState != null && this._currentClickBoard != null)
            {
                this.NotifyState(this._model.GetBoardState(this.ActiveBoardName), null);
            }
            base.Refresh();
        }

        
        private void AddTableLayoutPanel(TableLayoutPanel tbLayoutPanel)
        {
            int panelrow = this._boardRowNumber;
            int panelcol = this._boardColNumber + 1;
            tbLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            tbLayoutPanel.ColumnCount = panelcol;
            tbLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tbLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tbLayoutPanel.Name = "BoardLayout";
            tbLayoutPanel.RowCount = panelrow;
            tbLayoutPanel.Size = new System.Drawing.Size(500, 500);
            tbLayoutPanel.TabIndex = 0;
            float widthfirst = 50;
            float widthPercent = (float)((100.0 ) / panelcol) - 1;
            float heightPercent = (float)(100.0 / panelrow) - 1;
            tbLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, widthfirst));
            for (int col = 1; col < panelcol; col++)
                tbLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, widthPercent));
            for (int row = 0; row < panelrow; row++)
                tbLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, heightPercent));

            this.SuspendLayout();
            this.Controls.Add(tbLayoutPanel);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void BoardUC_Load(object sender, EventArgs e)
        {
            int panelrow = this._boardRowNumber;
            int panelcol = this._boardColNumber + 1;
            TableLayoutPanel tbLayoutPanel = new TableLayoutPanel();
            AddTableLayoutPanel(tbLayoutPanel);
            tbLayoutPanel.SuspendLayout();
            this._boardList = new BoardCell[panelrow, panelcol];
            for (int row = panelrow - 1; row > -1; row--) 
                for (int col = 0; col < panelcol; col++)
                    ConfigureBoardCell(out this._boardList[row, col], row, col, tbLayoutPanel);
            tbLayoutPanel.ResumeLayout(false);
            _redraw = true;
        }

        
        private void ConfigureBoardCell(out BoardCell bc, int row, int col,TableLayoutPanel tbLayoutPanel)
        {
            string controlName = "";
            bc = new BoardCell(row, col);
            bc.BackgroundPainter = PainterFactory.GetBoardStatePainter("UNSELECTED");
            bc.BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");
            bc.ContextMenuStrip = this.cmsRightClick;
            bc.Dock = System.Windows.Forms.DockStyle.Fill;
            bc.ProgressType = ProgressODoom.ProgressType.Smooth;
            bc.ShowPercentage = false;
            if (col != 0)
            {
                controlName = this._posMapScheme.GetBoardName(row+1, col);
                //controlName = 'L'+ (row + 1).ToString() + '-'+ col.ToString();
            }
            else
            {
                controlName = 'F' + (row + 1).ToString();
            }
            bc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProgressBarEx_MouseDown);
            bc.Text = controlName;
            bc.Name = "pbeBoardUC" + controlName;
            bc.Value = 0;
            tbLayoutPanel.Controls.Add(bc);
        }




        private void ActiveBoard(BoardCell bar, BoardState currentBoardState)
        {
            if (BoardState.UNSELECTED == currentBoardState)
                return;

            int panelrow = this._boardRowNumber;
            int panelcol = this._boardColNumber + 1;

            for (int i = panelrow - 1; i > -1; i--)
                for (int j = 0; j < panelcol; j++)
                    this._boardList[i,j].BorderPainter = PainterFactory.GetPlainBoarderPainter("UNACTIVE");

            
            bar.BorderPainter = PainterFactory.GetPlainBoarderPainter("ACTIVE");
            if (this.ActiveBoardEvent != null)
                this.ActiveBoardEvent(this, new BoardLocateEventArgs(bar.RowId, bar.ColId, bar.Text));
            if (this.NotifyState != null && this._currentClickBoard != null)
            {
                this.NotifyState(currentBoardState, null);
            }
        }

        private void ProgressBarEx_MouseDown(object sender, MouseEventArgs e)
        {
            BoardCell bc = (BoardCell)sender;
            if (bc.ColId == 0)
            {
                this.SwitchRightClickMenuMode(RightClickMenuMode.Unavailable);
                return;
            }
            this._currentClickBoard = bc;
            BoardState currentBoardState = this._model.GetBoardState(this.ActiveBoardName);
            UpdateRightClickMenuState(currentBoardState);

            MouseEventArgs a = (MouseEventArgs)e;
            if (a.Button == MouseButtons.Left)
                this.ActiveBoard((BoardCell)sender, currentBoardState);
        }
      

      
        private enum RightClickMenuMode
        {
            Available,
            Unavailable,
            OnlyDisable,
            RecoverOrDisable,
        };
        private void SwitchRightClickMenuMode(RightClickMenuMode mode)
        {
            switch (mode)
            {
                case RightClickMenuMode.Available:
                    this.cmsRightClick.Enabled = true;
                    this.cmsRightClick.Items[0].Enabled = true;
                    this.cmsRightClick.Items[1].Enabled = true;
                    this.cmsRightClick.Items[2].Enabled = false;
                    break;
                case RightClickMenuMode.Unavailable:
                    this.cmsRightClick.Enabled = false;
                    this.cmsRightClick.Items[0].Enabled = false;
                    this.cmsRightClick.Items[1].Enabled = false;
                    this.cmsRightClick.Items[2].Enabled = false;
                    break;
                case RightClickMenuMode.OnlyDisable:
                    this.cmsRightClick.Enabled = true;
                    this.cmsRightClick.Items[0].Enabled = true;
                    this.cmsRightClick.Items[1].Enabled = false;
                    this.cmsRightClick.Items[2].Enabled = false;
                    break;
                case RightClickMenuMode.RecoverOrDisable:
                    this.cmsRightClick.Enabled = true;
                    this.cmsRightClick.Items[0].Enabled = true;
                    this.cmsRightClick.Items[1].Enabled = false;
                    this.cmsRightClick.Items[2].Enabled = true;
                    break;
            }
        }
        private void cmsRightClick_Opened(object sender, EventArgs e)
        {
            this.enableToolStripMenuItem.DropDownItems.Clear();
            string[] plans = this._model.GetSupportPlanTable();
            if (plans != null)
            {
                foreach (var plan in plans)
                {
                    var item = new ToolStripMenuItem(plan);
                    item.Click += this.EnableToolStripMenuItem_Click;
                    this.enableToolStripMenuItem.DropDownItems.Add(item);
                }

            }    
        }
        private void DisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this._model.EraseBoard(this.ActiveBoardName);
            });
        }
        private void EnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            Task.Factory.StartNew(() =>
            {
                this._model.SetBoardEnable(this.ActiveBoardName, menuItem.Text);
            });
        }
        private void UpdateRightClickMenuState(BoardState state)
        {
            switch (state)
            {
                case BoardState.UNSELECTED:
                case BoardState.SELECTED:
                    this.SwitchRightClickMenuMode(RightClickMenuMode.Available);
                    break;
                case BoardState.CONFLICT:
                    this.SwitchRightClickMenuMode(RightClickMenuMode.RecoverOrDisable);
                    break;
                case BoardState.DONE:
                case BoardState.LOADED:
                case BoardState.READY:
                    this.SwitchRightClickMenuMode(RightClickMenuMode.OnlyDisable);
                    break;
                case BoardState.RUNNING:
                    this.SwitchRightClickMenuMode(RightClickMenuMode.Unavailable);
                    break;
            }
        }
        public string ActiveBoardName
        {
            get { return this._currentClickBoard.Text; }
        }

        public EventHandler ActiveBoardEvent
        {
            get
            {
                return _activeBoardEvent;
            }

            set
            {
                _activeBoardEvent = value;
            }
        }

        public EventHandler NotifyState
        {
            get
            {
                return _notifyState;
            }

            set
            {
                _notifyState = value;
            }
        }

        private void recoverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.Recover(this.ActiveBoardName);
        }
    }
}

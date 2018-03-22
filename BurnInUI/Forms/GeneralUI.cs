using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using BILib;
using BIModel;
using BIModel.Interface;
using BurnInUI.ConfigReader;
using System.Configuration;
using System.Reflection;

namespace BurnInUI.Forms
{
    public partial class GeneralUi:Form
    {
      
        private IBiModel _model;
        private log4net.ILog _logger;
        private ConfigInfo _configInfo;
        private UPSUtility _ups;
        private LoadConfig _reader;
        private IDatabaseService _dataService;
        private string _userId;
        private string _userPassword;
        private string _localConStr;
        private string _remoteConStr;
        public GeneralUi()
        {
            InitializeComponent();
         
        }
        private void General_Load(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _logger = log4net.LogManager.GetLogger("TextBoxLog");
            Text = $@"BMS(V{version.ToString()})";
            InitForm.Instance().Show();

            try
            {
                LoadConfig();
                InitDataBase();
                InitUi();
                EnableMenue(false);
                btnBinding.Enabled = false;
                btnContinue.Enabled = false;
                btnPause.Enabled = false;
                btnCheckConnection.Enabled = false;
                if (_configInfo.UpsSwitch == "ON")
                {
                    _ups.UpsWarningTimeOut += OnUpsWarningTimeOut;
                    _ups.StartUPS();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"App start fail: {ex.Message}");
            }
            finally
            {
                InitForm.Instance().Close();
            }
        }
        
        private void LoadConfig()
        {
            _reader = new LoadConfig(_logger);
            _configInfo = _reader?.LoadConfigParam();
        }
        private void InitDataBase()
        {
            _logger.Info("Start remote bak service");
            
            _localConStr = ConfigurationManager.AppSettings.Get("connectionstring");
            if (_localConStr == null)
            {
                _localConStr = $"Data Source = {_configInfo.DataSource}; Initial Catalog = {_configInfo.DataBaseName}; Persist Security Info = True; User ID = { _configInfo.UserId}; Password = {_configInfo.Password}; Pooling = False";
            }   
            _dataService = DataServiceFactory.CreateDatabaseService(_localConStr);
            _dataService.SetBakupPeriod(_configInfo.BakupPeriodMinutes);
            _dataService.SetDeletePeriod(_configInfo.DeletePeriodDays);

            _remoteConStr = $"Data Source = {_configInfo.RemoteDataSource}; Initial Catalog = {_configInfo.RemoteDataBaseName}; Persist Security Info = True; User ID = { _configInfo.RemoteUserId}; Password = {_configInfo.RemotePassword}; Pooling = False";
            _dataService.SetRemoteConStr(_remoteConStr);

            

            var isExist = true;
            if (!_dataService.IsTableExist(_localConStr, _configInfo.LocalBiDataSummaryTable))
            {
                isExist = false;
                _logger.Info(_configInfo.LocalBiDataSummaryTable + " is not exist");
            }
            if (!_dataService.IsTableExist(_remoteConStr, _configInfo.RemoteSummaryBiDataTable))
            {
                isExist = false;
                _logger.Info(_configInfo.RemoteSummaryBiDataTable + " is not exist");
            }
            if (!_dataService.IsTableExist(_localConStr, _configInfo.LocalBiDataTable))
            {
                isExist = false;
                _logger.Info(_configInfo.LocalBiDataTable + " is not exist");
            }
            if (!_dataService.IsTableExist(_remoteConStr, _configInfo.RemoteBiDataTable))
            {
                isExist = false;
                _logger.Info(_configInfo.RemoteBiDataTable + " is not exist");
            }
            if(isExist)
                _dataService.StartBakService(true);

        }
        private void EnableMenue(bool enbale)
        {
            mappingToolStripMenuItem.Enabled = enbale;
            dataBaseToolStripMenuItem.Enabled = enbale;
            configToolStripMenuItem.Enabled = enbale; 
            
        }

        private void InitUi()
        {
            var posMapBlock = BiModelFactory.CreateIPosMapScheme(_dataService).GetDefaultPosMapBlock();
            tableLayoutPanel1.RowStyles[1].Height = posMapBlock.BoardRows / (float)(posMapBlock.BoardRows + posMapBlock.SeatRows);
            tableLayoutPanel1.RowStyles[2].Height = posMapBlock.SeatRows / (float)(posMapBlock.BoardRows + posMapBlock.SeatRows);
            _ups = UPSUtility.Instance(_logger, _configInfo);
            _model = BiModelFactory.GetBiModel(_logger, _dataService, GetModelParam(_configInfo));
            parameterShowUC.SetModel(_model);
            AddBoardUc();
            AddProductUc();
            boarduc.ActiveBoardEvent += productuc.OnBoardActive;
            productuc.NotifyActiveSn += OnNotifyActiveSn;
        }

        private void AddBoardUc()
        {
            boarduc = new BoardUc(_model, _dataService)
            {
                Location = new System.Drawing.Point(0, 0),
                Dock = DockStyle.Fill,
                Name = "boarduc"
            };
            panel3.Controls.Add(boarduc);
            boarduc.Size = new System.Drawing.Size(boarduc.Parent.Size.Width, 471);
            boarduc.NotifyState = OnNotifyStateUpdate;
        }
       
        private void OnNotifyStateUpdate(object sender, EventArgs e)
        {
            ChangeGuiState((BoardState)sender, _model.Executable());
        }

        private void AddProductUc()
        {
            productuc = new ProductUc(_model, _dataService);
            productuc.Dock = DockStyle.Fill;
            productuc.Name = "productuc";
            panel2.Controls.Add(productuc);
        }
       
        private static ConfigParam GetModelParam(ConfigInfo info)
        {
            var param = new ConfigParam
            {
                systemParam = new SystemParams(),
                ports = new Dictionary<string, string>()
            };
            param.systemParam.testStation = info.TestStation;
          
            param.systemParam.comErrorTolarence = info.ComErrorTolarence;
            param.systemParam.conditionTimeout = info.ConditionTimeout;
            param.systemParam.heatTime = info.HeatTime;
            param.systemParam.tbiesServer = info.TbiesServer;
           
            param.ports["OvenPort"] = info.OvenPort;
            param.ports["Floor1"] = info.Floor1Port;
            param.ports["Floor2"] = info.Floor2Port;
            param.ports["Floor3"] = info.Floor3Port;
            param.ports["Floor4"] = info.Floor4Port;
            param.ports["Floor5"] = info.Floor5Port;
            param.ports["Floor6"] = info.Floor6Port;
            param.ports["Floor7"] = info.Floor7Port;
            param.ports["Floor8"] = info.Floor8Port;
            param.ports["Floor9"] = info.Floor9Port;
            param.ports["Floor10"] = info.Floor10Port; 
            return param;
        }
        
        private void OnNotifyActiveSn(object sender, EventArgs args)
        {
            string sn = sender.ToString();
            parameterShowUC.SetSN(sn);
        }
   

        

        private void OnUpsWarningTimeOut(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                _model.PauseAll();
            });
        }


        private Dictionary<int, string> RequestBindingSn(string board)
        {
            string[] snlist;
            using (var a = new SNBindingDialog(board, _model,_configInfo))
            {
                a.ShowDialog();
                snlist = a.GetSNList();
            }

            var snSet = new Dictionary<int, string>();
            for (int i = 0; i < snlist.Length; i++)
            {
                if (!string.IsNullOrEmpty(snlist[i]))
                {
                    int seat = i + 1;
                    snSet[seat] = snlist[i];
                }
            }
            return snSet;
        }
        private void btnBinding_Click(object sender, EventArgs e)
        {
            _logger.Info("Binding SN Start...");
            btnBinding.Enabled = false;
            foreach (var board in _model.GetSelectedBoard())
            {
                var snSet = _model.GetSnSet(board) ?? RequestBindingSn(board);
                _model.BindingSnOnBoard(board, snSet);
            }

            _model.CreateScanResultFile();
            _logger.Info("Binding SN End...");
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(@"Are you sure to cut down product’s current?  \r\n 确定要对该板子暂停监控？", @"Stop Burnin", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                btnPause.Enabled = false;
                _logger.Info("Pause Board Start...");
                Task.Factory.StartNew(() =>
                {
                    _model.PauseBoard(boarduc.ActiveBoardName);
                    _logger.Info("Pause Board End...");
                });
            }
        }
        
        private void btnContinue_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = false;
            _logger.Info("Start Burn In Start...");
            Task.Factory.StartNew(() =>
            {
                _model.StartBurnIn();
                _logger.Info("Start Burn In Done...");
            });
        }
        
      
        public void ChangeGuiState(BoardState state, bool executable)
        {
            if(executable==false)
                state = BoardState.UNSELECTED;
            switch (state)
            {
                case BoardState.SELECTED:
                    btnBinding.Enabled = true;
                    btnCheckConnection.Enabled = false;
                    btnContinue.Enabled = false;
                    btnPause.Enabled = false;
                    break;
                case BoardState.LOADED:
                    btnBinding.Enabled = false;
                    btnCheckConnection.Enabled = true;
                    btnContinue.Enabled = false;
                    btnPause.Enabled = false;
                    break;
                case BoardState.READY:
                    btnBinding.Enabled = false;
                    btnCheckConnection.Enabled = false;
                    btnContinue.Enabled = true;
                    btnPause.Enabled = false;
                    break;
                case BoardState.RUNNING:
                    btnBinding.Enabled = false;
                    btnCheckConnection.Enabled = false;
                    btnContinue.Enabled = false;
                    btnPause.Enabled = true;
                    break;
                case BoardState.DONE:
                case BoardState.CONFLICT:
                case BoardState.UNSELECTED:
                    btnBinding.Enabled = false;
                    btnCheckConnection.Enabled = false;
                    btnContinue.Enabled = false;
                    btnPause.Enabled = false;
                    break;
            }
        }
        

        
        private void GeneralUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(@"Are you sure to close the UI ? All the system will be close！", @"System message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                _dataService.StartBakService(false);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            
            btnCheckConnection.Enabled = false;
            _logger.Info("Check Connection Start...");
            Task.Factory.StartNew(() =>
            {
                _model.CheckAllConnection();
                _logger.Info("Check Connection End...");
            });
        }


        private void specAssisantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = Process.Start("SpecEditor.exe");
            p?.WaitForExit();
        }
        private void dataToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = Process.Start("DataTool.exe");
            p?.WaitForExit();
        }
        private void scanConToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = Process.Start("ScanCocTool.exe");
            p?.WaitForExit();
        }
        private void rEADMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = Process.Start("ReadMe.txt");
            p?.WaitForExit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About.Instance().Show();
        }

        private void mappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapUi.Instance(_dataService).Show();

        }

        private void dataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseControl.Instance(_dataService,_configInfo).Show();
        }

        private void configToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ConfigUi.Instance(_reader).Show();
        }

        

        private void enginnerModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var login = new LoginForm
            {
                UserId = _userId,
                UserPassword = _userPassword
            };
            login.ShowDialog();
            if (login.LoginResult)
            {
                _logger.Info("Enter into engineer mode");
                Text = $@"BMS(V{version.ToString()})--Welcome,"+login.UserId;
                EnableMenue(true);
            }
            else
            {
                EnableMenue(false);
                Text = $@"BMS(V{version.ToString()})";
                _logger.Info("Off engineer mode");
            }
           
            _userId = login.UserId;
            _userPassword =login.UserPassword;
            
        }
    }
}

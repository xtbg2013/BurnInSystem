using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BILib;
using TbiesIntfDll;
using BIModel.Data;
using BiInterface;

using BIModel.Interface;
using BIModel.Factory;
using BIModel.runtime;



namespace BIModel.Manager
{
    public delegate void CompeteAction();
    public delegate void BoardAction(string boardName);
    class BIModelTOSA:IBiModel
    {
        private System.Timers.Timer BurninTimer = new System.Timers.Timer(30000);
        private System.Timers.Timer BurninTimercost = new System.Timers.Timer(30000);
        private log4net.ILog log = null;
        private IMesOperator _mesOperator;
        private IDatabaseService _dbService;
        private BoardManager boardManager = null;
        private UnitManager unitManager = null;
        private FetchPlans _fetchPlans;
        private BoardFactory _boardFactory;
        private ConfigParam _configParam;
        
        


        public BIModelTOSA(ConfigParam param,log4net.ILog logger, IDatabaseService dbStore)
        {
            _configParam = param;
            log = logger;
            _dbService = dbStore;

            try
            {
                _fetchPlans = FetchPlans.Inst(_configParam.systemParam.tbiesServer);
                _boardFactory = BoardFactory.Instance(log, _fetchPlans, _configParam.ports);
                _mesOperator = BiModelFactory.GetMesOperator();
                boardManager = new BoardManager(log, dbStore, BiModelFactory.CreateIPosMapScheme(dbStore),  _boardFactory, param.systemParam);
                unitManager = new UnitManager(log, param.systemParam);

                SyncDbStore(dbStore);
                InitTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            } 
        }
        private void InitTimer()
        { 
            BurninTimer = new System.Timers.Timer(30000);
            BurninTimercost = new System.Timers.Timer(30000);
            BurninTimer.Interval = 60000;
            BurninTimercost.Interval = 60000;
            BurninTimer.Elapsed += ParameterMonitorTick;
            BurninTimercost.Elapsed += CostTimeCalculateTick;
            BurninTimer.Start();
            BurninTimercost.Start();
        }
        public ConfigParam GetModelParam()
        {
            return _configParam;
        }
        public string[] GetSupportPlanTable()
        {
           
            IDriverFactory factroy = _boardFactory.CreateDriverFactory();
            if (factroy != null)
            {
                return _fetchPlans.GetSupportPlanTable(factroy.GetSupportDriver());
            }
            else
            {
                return null;
            }
                
        }
        public SpecItem[] GetSpecByPlan(string plan)
        {
            return _fetchPlans.GetSpecification(plan);
        }
      
        public bool IsUnitExist(string sn)
        {
            bool res = true;
            if (unitManager.GetUnitDetail(sn).Count > 0)
            {
                if (unitManager.GetUnitDetail(sn)["UnitState"].ToUpper() != "DONE")
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
            else
                res = false;
            return res;
        }
        public IBoard GetController(string boardName)
        {
           return boardManager.GetBoardInfo(boardName).GetController();
        }
        #region Public Init & Get

        private void SyncDbStore(IDatabaseService dbStore)
        {
            _fetchPlans.FetchPlansList(dbStore);
            boardManager.SyncFromStore();
            unitManager.SyncFromStore(dbStore);
        }
        
        public string GetBoardName(int row, int col)
        {
            return ((char)('A' + row)).ToString() + (col + 1);
        }

        public BoardState GetBoardState(string boardName)
        {
            var info = boardManager.GetBoardInfo(boardName);
            if (info == null)
                return BoardState.UNSELECTED;
            return info.Flag;
        }

        public int GetBoardSeatsCount(string boardName)
        {
            var info = boardManager.GetBoardInfo(boardName);
            if (info == null)
                return 0;
            IBoard board = info.GetController();
            if (board != null)
                return board.SeatsCount;
            else
                return 0;
        }

        public int GetProgress(string sn)
        {
            return  unitManager.GetProgress(sn);
        }

        public string GetSnByPos(string boardName, int seat)
        {
            return unitManager.GetSn(boardName, seat);
        }

        public string[] GetProductStateOnBoard(string boardName)
        {
            int dutCount = GetBoardSeatsCount(boardName);
            string[] productStatus = new string[dutCount];
            for (int k = 0; k < dutCount; k++)
            {
                productStatus[k] = null;
                string sn = unitManager.GetSn(boardName, k + 1);
                productStatus[k] = unitManager.GetUnitStyle(sn);
            }
            return productStatus;
        }

        public Dictionary<string, string> GetProductInformationBySn(string sn)
        {
            return unitManager.GetUnitDetail(sn);
        }

        #endregion

        #region Public Control
        public bool Executable()
        {
            return idleFlag;
        }

        public void Recover(string boardName)
        {
            boardManager.ChangeBoardState(boardName, BoardState.LOADED);
        }
        public void SetBoardEnable(string boardName, string plan)
        {
            log.Info("Enable Board:" + boardName);
            WaitToExecute(() =>
            {
                if(boardManager.GetBoardInfo(boardName)==null)
                    boardManager.SubscribeBoard(boardName, plan);
            }, true);
        }
        public void EraseBoard(string boardName)
        {
            log.Info("Erase Board:"+boardName);
            WaitToExecute(() =>
            {
                BoardState state = GetBoardState(boardName);
                switch (state)
                {
                    case BoardState.LOADED:
                    case BoardState.READY:
                    case BoardState.CONFLICT:
                    case BoardState.DONE:
                        foreach (var sn in unitManager.GetUnitSnSet(boardName))
                        {
                            if (state != BoardState.DONE)
                            {
                                unitManager.UpdateUnitResult(sn, UnitResult.FAIL, "User Abort");
                                unitManager.UpdateUnitState(sn, UnitState.DONE);
                            }
                            RemoveSingleUnit(sn);
                        }
                        break;
                }
                boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
                boardManager.UnsubscribeBoard(boardName);
            },true);
        }
        public void UnitAbort(string boardName, int seat)
        {
            string sn = unitManager.GetSn(boardName, seat);
            log.Info("Remove Unit:"+sn);
            WaitToExecute(() =>
            {
                unitManager.UpdateUnitResult(sn, UnitResult.FAIL, "User Abort");
                unitManager.UpdateUnitState(sn, UnitState.DONE);
                RemoveSingleUnit(sn);
                if (unitManager.GetUnitSnSet(boardName).Any() == false)
                    boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
            },true);
        }

        public void CheckAllConnection()
        {
            log.Info("Start Check Connection");
            WaitToExecute(() =>
            {
                Parallel.ForEach(boardManager.SelectBoardByState(BoardState.LOADED), name =>
                {
                    ExceptionControl(CheckDutsOnBoard, name);
                });
            },true);
        }
        public void StartBurnIn()
        {
            log.Info("Start Burn In");
            WaitToExecute(() =>
            {
                var boardToStart = boardManager.SelectBoardByState(BoardState.READY);
                Parallel.ForEach(boardToStart, x =>
                {
                    unitManager.UpdateStartBurnTime(x);
                    SetUpTemperature(x);
                    WaitForBurnInTemperature(x);
                    SetUpBoardElectric(x);
                    StartBurnInOnBoard(x);
                });
            },true);
        }
        public void PauseAll()
        {
            log.Info("Start Pause All");
            WaitToExecute(() =>
            {
                var boardToPause = boardManager.SelectBoardByState(BoardState.RUNNING);
                Parallel.ForEach(boardToPause, x =>
                {
                    TearDownBoardElectric(x);
                    TearDownTemperature(x);
                    SetBoardPause(x);
                });
            },true);
        }
        public void PauseBoard(string boardName)
        {
            log.Info("Start Pause Board:"+boardName);
            WaitToExecute(() =>
            {
                if (boardManager.GetBoardInfo(boardName).Flag == BoardState.RUNNING)
                {
                    TearDownBoardElectric(boardName);
                    TearDownTemperature(boardName);
                    SetBoardPause(boardName);
                }
            },true);
        }
        private void ScanBoardDone(List<string> doneSet)
        {
            log.Info("Start Set Board Done");
            WaitToExecute(() =>
            {
                UpdateDutsState(doneSet, UnitState.DONE);

                List<string> boardSetDone = new List<string>();
                foreach(string name in boardManager.SelectBoardByState(BoardState.RUNNING))
                    if(unitManager.GetUnitSnSet(name,UnitState.BURNIN).Any()==false)
                        boardSetDone.Add(name);

                Parallel.ForEach(boardSetDone, x =>
                {
                    try
                    {
                        var result = boardManager.GetBoardInfo(x).GetController().CheckConnections(true);
                        if (result != null)
                        {
                            foreach (var idx in result.Keys)
                            {
                                if (result[idx] == false)
                                {
                                    unitManager.UpdateUnitResult(unitManager.GetSn(x, idx), UnitResult.FAIL, "ReTest Failed");
                                }
                            }
                        }

                        TearDownBoardElectric(x);
                        TearDownTemperature(x);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    finally
                    {
                        boardManager.ChangeBoardState(x, BoardState.DONE);
                    }
                });
            },true);
        }
        private void ParameterMonitorTick(object sender, EventArgs e)
        {
            try
            {
                BurninTimer.Stop();
                var nameSet = boardManager.SelectBoardByState(BoardState.RUNNING);
                if (nameSet.Any())
                    WaitToExecute(() =>
                    {
                        Parallel.ForEach(nameSet, name => { ExceptionControl(ReadDataOnBoard, name); });
                    }, false);
            }
            catch (Exception ex)
            {
                log.Warn("ParameterMonitorTick: " + ex.ToString());
            }
            finally
            {
                BurninTimer.Start();
            }
        }

        #region Using WaitToExecute 
        private readonly object lockWaitToExecute = new object();
        private volatile bool idleFlag = true;
      
        private void WaitToExecute(CompeteAction action, bool logEnabled)
        {
            var timer = new System.Timers.Timer(5000);
            timer.Elapsed+= (s, e) =>
            {
                if (logEnabled)
                    log.Info("Wait for System Idle...");
            };
            timer.Start();
            lock (lockWaitToExecute)
            {
                try
                {
                    idleFlag = false;
                    timer.Stop();
                    action();
                }
                finally
                {
                    idleFlag = true;
                    timer.Dispose();
                }
            }
        }
        #endregion

        #endregion

        #region Public Method
        
        public EventHandler ProductsUpdate
        {
            get { return unitManager.ProductsUpdate; }
            set { unitManager.ProductsUpdate = value; }
        }

        public EventHandler BoardStateChanged
        {
            get { return boardManager.BoardStateChanged; }
            set { boardManager.BoardStateChanged = value; }
        }
        
        #endregion

       
        private void CostTimeCalculateTick(object sender, EventArgs e)
        {
            try
            {
                BurninTimercost.Stop();
                UpdateCostTime();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            finally
            {
                BurninTimercost.Start();
            }
        }

     
        
        #region For Binding SN
        public void CreateScanResultFile()
        {
            using (var sr = System.IO.File.CreateText(Utility.GetDefaultSnFile()))
            {
                foreach (string sn in unitManager.GetAllSnSet())
                    sr.WriteLine(sn);
                sr.Close();
            }

            foreach (string name in boardManager.SelectBoardByState(BoardState.SELECTED))
                foreach (string sn in unitManager.GetUnitSnSet(name))
                    log.Info(name + "\t" + sn);
        }
        #endregion
        
        #region For Start Burn In
        private void UpdateCostTime()
        {
            foreach (var name in boardManager.SelectBoardByState(BoardState.RUNNING))
            {
                List<string> doneSn = new List<string>();
                var result = unitManager.GetUnitSnSet(name,UnitState.BURNIN);
                doneSn.AddRange(result.FindAll(sn =>
                {
                        unitManager.UpdateCostTime(sn);
                        return unitManager.CheckTimeOut(sn);
                }));

                if (doneSn.Count > 0)
                {
                    ScanBoardDone(doneSn);

                    if (boardManager.GetBoardInfo(name).GetController().IsMesCheck())
                    {
                        MoveNextStation(doneSn);
                    }
                }
            }
        }

        private void Save_And_Check_Data(string sn, Dictionary<int, List<KeyValuePair<string, string>>> data)
        {
            string board;
            int seat;
            unitManager.GetBoardSeat(sn,out board,out seat);
            var dataSet = data[seat];
            try
            {
                unitManager.UpdateParaTime(sn);
                unitManager.UploadData(sn,dataSet);
                string message = "";
                SpecResult ret = unitManager.CheckDataBySpec(sn, dataSet, out message);
                bool doStop;
                unitManager.DealWithSpecResult(sn,ret,message,out doStop);
                IBoard controller = boardManager.GetBoardInfo(board).GetController();
                if (controller == null)
                {
                    log.Warn(sn + " stop fail ");
                }
                else
                {
                    if (doStop)
                        controller.CatchException(seat);
                    if (ret.Stopable)
                    {
                        if (controller.IsMesCheck())
                        {
                            //Add for MES function, defause values "", if value not null, no need the MES function
                            if (controller.IsHold())
                            {
                                try
                                {
                                    string retMsg = "";
                                    if (_mesOperator.Hold(sn, "NG Auto Hold", out retMsg))
                                    {
                                        log.Info("The " + sn + " Auto Hold in this station.");
                                    }
                                    else
                                    {
                                        log.Warn(sn + " Fail to Hold.Reason is " + retMsg);
                                    }
                                }
                                catch
                                {
                                    log.Warn(sn + " Fail to Hold for MES reason.");
                                };
                            }
                            else
                            {
                                log.Info("The " + sn + " user no need to hold.");
                            }
                        }
                        else
                        {
                            log.Info("The " + sn + " no need to check MES Action.");
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {
                log.Error(sn + ":" + ex.Message);
            }
        }
        
        private void UpdateDutsState(List<string> snSet, UnitState nextState)
        {
            foreach (string sn in snSet)
                unitManager.UpdateUnitState(sn, nextState);
        }

        private void RaiseProductsUpdate()
        {
            ProductsUpdate?.Invoke(this, null);
        }

        private void WaitForBurnInTemperature(string boardName="")
        {
            log.Info(boardName+" "+"Wait For Burn-In Temperature...");
            int heatTime = _configParam.systemParam.heatTime;//int.Parse(SettingsReader.GetInstance().GetSettingsValue("OtherSetting", "HEATTIME"));
            while (heatTime>0)
            {
                log.Info(boardName + " "+"Remain:\t" + heatTime + " min");
                if (heatTime > 1)
                    Thread.Sleep(60000);
                else
                    for (int i = 59; i >= 0; i--)
                    {
                        Thread.Sleep(1000);
                        if(i%10==0||i<=5)
                            log.Info(boardName + " " + "Remain:\t" + i + " sec");
                    }
                heatTime--;
            }
        }

        #endregion

        #region For Product UI
        private void RemoveSingleUnit(string sn)
        {
            string board;
            int seat;
            unitManager.GetBoardSeat(sn, out board,out seat);
            boardManager.RemoveSeatFromBoard(board,seat);
            unitManager.UnsubscribeUnit(sn);
        }
        
        #endregion

        #region Using Exception Control

        private void ExceptionControl(BoardAction action, string boardName)
        {
            try
            {
                action(boardName);
            }
            catch (Exception ex)
            {
                log.Error(boardName + ":" + ex.Message);
                log.Error(boardName + ex.StackTrace);
                boardManager.ChangeBoardState(boardName, BoardState.CONFLICT);
            }
        }

        #region interface for SN Binding
        public List<string> GetSelectedBoard()
        {
            return boardManager.SelectBoardByState(BoardState.SELECTED);
        }
        public int GetSeatsCount(string boardName)
        {
            return boardManager.GetBoardInfo(boardName).GetController().SeatsCount;
        }
        public Dictionary<int, string> GetSnSet(string boardName)
        {
            return boardManager.GetBoardInfo(boardName).GetController().GetSnSet();/* what would be returned if sn are binded mannually*/
        }
        private BoardEntry CreateBoardEntry(string boardName, Dictionary<int, string> snSet)
        {

            BoardEntry board = boardManager.GetBoardInfo(boardName);
            return board;
        }
        private void _BindingSnOnBoard(string boardName, Dictionary<int, string> snSet)
        {
            if (snSet != null && snSet.Count != 0)
            {
                BoardEntry board = boardManager.GetBoardInfo(boardName);
                double OvenTargetTemperature = board.GetController().GetTargetOvenTemperature();
                foreach (var seat in snSet.Keys)
                {
                    string sn = snSet[seat];
                    boardManager.AddSeatToBoard(boardName, seat, sn);
                    unitManager.SubscribeUnit(sn, board.Plan, OvenTargetTemperature, boardName, seat);       
                }
            }
            if (unitManager.GetUnitSnSet(boardName).Any() == false)
            {
                log.Warn(boardName + " is Empty.");
                boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
                boardManager.UnsubscribeBoard(boardName);
            }
            else
            {
                boardManager.ChangeBoardState(boardName, BoardState.LOADED);
            }
        }
        public void BindingSnOnBoard(string boardName, Dictionary<int, string> snSet)
        {
            WaitToExecute(() => {
                ExceptionControl(delegate (string board)
                {
                    _BindingSnOnBoard(board, snSet);

                }, boardName);
            }, true);
        }
        #endregion
        private void CheckDutsOnBoard(string boardName)
        {
            bool checkPass = true;
            var result = boardManager.GetBoardInfo(boardName).GetController().CheckConnections(false);
            List<string> failInfoSet=new List<string>();
            foreach (int seat in result.Keys)
            {
                if (result[seat] == false)
                {
                    checkPass = false;
                    failInfoSet.Add(unitManager.GetSn(boardName,seat));
                }
            }
            if(checkPass==false)
                UpdateDutsState(failInfoSet,UnitState.REWORK);
            else
                UpdateDutsState(unitManager.GetUnitSnSet(boardName,UnitState.REWORK), UnitState.READY);
            boardManager.ChangeBoardState(boardName, checkPass ? BoardState.READY : BoardState.LOADED);
            log.Info(boardName + ":" + (checkPass ? " PASS" : " FAIL"));

        }

        private void StartBurnInOnBoard(string boardName)
        {
            UpdateDutsState(unitManager.GetUnitSnSet(boardName), UnitState.BURNIN);
            boardManager.ChangeBoardState(boardName, BoardState.RUNNING);
        }
        
        private void SetBoardPause(string boardName)
        {
            var locateSet = unitManager.GetUnitSnSet(boardName);
            UpdateDutsState(locateSet, UnitState.PAUSE);
            boardManager.ChangeBoardState(boardName, BoardState.LOADED);
        }
        
        private void ReadDataOnBoard(string boardName)
        {
            List<string> readList = unitManager.GetUnitSnSet(boardName,UnitState.BURNIN).FindAll(sn=>unitManager.ReadActive(sn));
            if (readList.Count > 0)
            {
                var boardInfo = boardManager.GetBoardInfo(boardName);
                try
                {
                    var data = boardInfo.GetController().ReadDataSet("RUN");
                    boardInfo.ErrorCount = 0;
                    foreach (string sn in readList)
                        Save_And_Check_Data(sn, data);
                }
                catch(Exception ex)
                {
                    log.Error(boardName+":"+ex.Message);
                    boardInfo.ErrorCount += 1;
                    if (boardInfo.ErrorCount > _configParam.systemParam.comErrorTolarence)
                        throw new Exception("Above Com Error Tolarence");
                }
            }
        }

        #endregion
    
        private bool SetUpTemperature(string boardName)
        {
            double temperature =  boardManager.GetBoardInfo(boardName).GetController().GetTargetOvenTemperature();
            //double temperature =  LogicPool.Inst().MapTemperatureByPlan(boardManager.GetBoardInfo(boardName).Plan);
            
            var ret = boardManager.GetBoardInfo(boardName).GetController().SetUpTemperature(temperature);
            log.Info(boardName + " Setup Temperature " + (ret ? "Arrived." : "Unarrived."));
            return ret;
        }

        private bool TearDownTemperature(string boardName)
        {
            var ret = boardManager.GetBoardInfo(boardName).GetController().TearDownTemperature();
            log.Info(boardName + " TearDown Temperature " + (ret ? "Arrived." : "Unarrived."));
            return ret;
        }

        private bool SetUpBoardElectric(string boardName)
        {
            var ret = boardManager.GetBoardInfo(boardName).GetController().EnableBoard();
            log.Info(boardName + " SetUp Electric " + (ret ? "Finished." : "Unfinished."));
            return ret;
        }

        private bool TearDownBoardElectric(string boardName)
        {
            var ret = boardManager.GetBoardInfo(boardName).GetController().DisableBoard();
            log.Info(boardName + " TearDown Electric " + (ret ? "Finished." : "Unfinished."));
            return ret;
        }

        #region For ParameterUC

        public string[] GetUnitParaSet(string sn)
        {
            return unitManager.GetUnitParaSet(sn);
        }

        public DataTable FetchUnitData(string sn, int range = 200)
        {
            return unitManager.FetchUnitData(sn, range);
        }

        #endregion
        #region MES Part
        private bool MoveNextStation(List<string> dataList)
        {
            bool flag = true;
            if ((dataList.Count > 0))
            {
                foreach (string locate in dataList)
                {
                    string sn = locate;
                    string board;
                    int seat;
                    unitManager.GetBoardSeat(sn, out board, out seat);
                    IBoard controller = boardManager.GetBoardInfo(board).GetController();
                    int holdormove;
                    if (_mesOperator.AutoMoveOut(sn, unitManager.GetUnitDetail(sn)["UnitResult"], controller.IsHold(), out holdormove))
                    {
                        //This Pass SN normally Move next station
                        if (holdormove == 1)
                        {
                            log.Info("The " + sn + " Auto moved next test station.");
                        }
                        //The Fail SN normally Hold on this station
                        if (holdormove == 2)
                        {
                            log.Info("The " + sn + " Auto Hold in this station.");
                        }
                        //For the Failed SN, user no need to hold
                        if (holdormove == 3)
                        {
                            log.Info("The " + sn + " No need to hold in this station.");
                        }

                    }
                    else
                    {
                        //Call the SN MES move or hold function abnormal
                        log.Info("The " + sn + " Failed to move next test station!");
                        flag = false;
                    }
                }
            }
            return flag;
        }
        #endregion
    }
}


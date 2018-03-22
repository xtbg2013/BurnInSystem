using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BILib;
using BiInterface;

using TbiesIntfDll;
using BIModel.Data;
using BIModel.Interface;
using BIModel.Factory;
using BIModel.Manager;
using log4net;

namespace BIModel.NowCoding
{
    public delegate void CompeteAction();
    public delegate void BoardAction(string boardName);
    class BIModelTOSA //: IBiModel
    {
        private System.Timers.Timer BurninTimer = new System.Timers.Timer(30000);
        private System.Timers.Timer BurninTimercost = new System.Timers.Timer(30000);
        private ILog _logger;
        private ConfigParam _configParam;
        private IDatabaseService _dataService;

        private IMesOperator _mesOperator;
        private BoardManager _boardManager;
        private FetchPlans _fetchPlans;
        private BoardFactory _boardFactory;
        private readonly object lockWaitToExecute = new object();
        private volatile bool idleFlag = true;

        private void WaitToExecute(CompeteAction action, bool logEnabled)
        {
            var timer = new System.Timers.Timer(5000);
            timer.Elapsed += (s, e) =>
            {
                if (logEnabled)
                    this._logger.Info("Wait for System Idle...");
            };
            timer.Start();
            lock (this.lockWaitToExecute)
            {
                try
                {
                    this.idleFlag = false;
                    timer.Stop();
                    action();
                }
                finally
                {
                    this.idleFlag = true;
                    timer.Dispose();
                }
            }
        }
        private void InitTimer()
        {
            BurninTimer = new System.Timers.Timer(30000);
            BurninTimercost = new System.Timers.Timer(30000);
            this.BurninTimer.Interval =  60000;
            this.BurninTimercost.Interval =  60000;
            BurninTimer.Elapsed += this.ParameterMonitorTick;
            BurninTimercost.Elapsed += this.CostTimeCalculateTick;
            BurninTimer.Start();
            BurninTimercost.Start();
        }
        private void ScanBoardDone(List<string> doneSet)
        {
            this._logger.Info("Start Set Board Done");
            this.WaitToExecute(() =>
            {
                UpdateDutsState(doneSet, UnitState.DONE);
                List<string> boardSetDone = new List<string>();
                foreach (string name in this._boardManager.SelectBoardByState(BoardState.RUNNING))
                {
                    var unit = this._boardManager.GetBoardUnit(name);
                    if (unit != null)
                    {
                        if (unit.GetUnitSnSet(UnitState.BURNIN).Any() == false)
                            boardSetDone.Add(name);
                    }
                }
                Parallel.ForEach(boardSetDone, x =>
                {
                    try
                    {
                        TearDownBoardElectric(x);
                        TearDownTemperature(x);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        this._boardManager.ChangeBoardState(x, BoardState.DONE);
                    }
                });
            }, true);
        }
        private void ParameterMonitorTick(object sender, EventArgs e)
        {
            try
            {
                BurninTimer.Stop();
                var nameSet = this._boardManager.SelectBoardByState(BoardState.RUNNING);
                if (nameSet.Any())
                    this.WaitToExecute(() =>
                    {
                        Parallel.ForEach(nameSet, name => { this.ExceptionControl(this.ReadDataOnBoard, name); });
                    }, false);
            }
            catch (Exception ex)
            {
                this._logger.Warn("ParameterMonitorTick: " + ex.ToString());
            }
            finally
            {
                BurninTimer.Start();
            }
        }
        private void CostTimeCalculateTick(object sender, EventArgs e)
        {
            try
            {
                BurninTimercost.Stop();
                this.UpdateCostTime();
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message);
                this._logger.Error(ex.StackTrace);
            }
            finally
            {
                BurninTimercost.Start();
            }
        }

        public BIModelTOSA(ConfigParam param, ILog logger, IDatabaseService dataService)
        {
            this._configParam = param;
            this._logger = logger;
            this._dataService = dataService;
        }

        public void InitBiModel()
        {
            try
            {
                _fetchPlans = FetchPlans.Inst(_configParam.systemParam.tbiesServer);
                _fetchPlans.FetchPlansList(_dataService);

                _boardFactory = BoardFactory.Instance(_logger, _fetchPlans, _configParam.ports);
                _mesOperator = BiModelFactory.GetMesOperator();
                _boardManager = new BoardManager(_logger, _dataService, BiModelFactory.CreateIPosMapScheme(_dataService),_boardFactory, _configParam.systemParam);
                _boardManager.LoadBoardUnits();
                InitTimer();
            }
            catch (Exception ex)
            {
                this._logger.Error("BiModel init fail:" + ex.Message);
                this._logger.Error(ex.StackTrace);
            }
        }
        public ConfigParam GetModelParam()
        {
            return this._configParam;
        }
        public string[] GetSupportPlanTable()
        {
            IDriverFactory factroy = this._boardFactory.CreateDriverFactory();
            if (factroy != null)
            {
                return this._fetchPlans.GetSupportPlanTable(factroy.GetSupportDriver());
            }
            else
            {
                return null;
            }
        }
        public SpecItem[] GetSpecByPlan(string plan)
        {
            return this._fetchPlans.GetSpecification(plan);
        }
        public IBoard GetController(string boardName)
        {
            return this._boardManager.GetBoardUnit(boardName).GetController();
        }
        public string GetBoardName(int row, int col)
        {
            return ((char)('A' + row)).ToString() + (col + 1);
        }
        public BoardState GetBoardState(string boardName)
        {
            var info = this._boardManager.GetBoardUnit(boardName);
            if (info == null)
                return BoardState.UNSELECTED;
            return info.GetBoardInfo().Flag;
        }

        public int GetBoardSeatsCount(string boardName)
        {
            var info = this._boardManager.GetBoardUnit(boardName);
            if (info == null)
            {
                this._logger.Error(boardName + " is not exist");
                return 0;
            }

            IBoard board = info.GetController();
            if (board != null)
                return board.SeatsCount;
            else
            {
                this._logger.Error(boardName + " 's IBoard controller is null,get board seats count fail");
                return 0;
            }

        }

        public int GetProgress(string boardName, string sn)
        {
            var info = this._boardManager.GetBoardUnit(boardName);
            if (info != null)
            {
                return info.GetProgress(sn);
            }
            else
            {
                this._logger.Error(boardName + " ' is not exist,get " + sn + " progress fail");
                return 0;
            }
        }


        public string GetSnByPos(string boardName, int seat)
        {
            var info = this._boardManager.GetBoardUnit(boardName);
            if (info != null)
            {
                return info.GetSn(seat);
            }
            else
            {
                this._logger.Error(boardName + " ' is not exist,get " + seat + " 's sn fail");
                return null;
            }
        }


        public string[] GetProductStateOnBoard(string boardName)
        {
            int dutCount = this.GetBoardSeatsCount(boardName);
            string[] productStatus = new string[dutCount];
            var info = this._boardManager.GetBoardUnit(boardName);
            for (int k = 0; k < dutCount; k++)
            {
                productStatus[k] = null;
                string sn = GetSnByPos(boardName, k + 1);
                productStatus[k] = info.GetUnitStyle(sn);
            }
            return productStatus;
        }

        public Dictionary<string, string> GetProductInformationBySn(string boardName, string sn)
        {
            var info = this._boardManager.GetBoardUnit(boardName);
            if (info == null)
            {
                this._logger.Error(boardName + " is not exist");
                return null;
            }
            return info.GetUnitDetail(sn);
        }


        public bool Executable()
        {
            return this.idleFlag;
        }

        public bool IsUnitExist(string sn)
        {
            bool res = true;

            //if (this.unitManager.GetUnitDetail(sn).Count > 0)
            //{
            //    if (this.unitManager.GetUnitDetail(sn)["UnitState"].ToUpper() != "DONE")
            //    {
            //        res = true;
            //    }
            //    else
            //    {
            //        res = false;
            //    }
            //}
            //else
            //    res = false;
            return res;
        }
        public void Recover(string boardName)
        {
            this._boardManager.ChangeBoardState(boardName, BoardState.LOADED);
        }
        public void SetBoardEnable(string boardName, string plan)
        {
            this._logger.Info("Enable Board:" + boardName);
            this.WaitToExecute(() =>
            {
                if (this._boardManager.GetBoardUnit(boardName) == null)
                    this._boardManager.SubscribeBoard(boardName, plan);
            }, true);
        }
        public void EraseBoard(string boardName)
        {
            this._logger.Info("Erase Board:" + boardName);
            var unit = this._boardManager.GetBoardUnit(boardName);
            if (unit == null)
            {
                return;
            }
            this.WaitToExecute(() =>
            {
                BoardState state = this.GetBoardState(boardName);
                switch (state)
                {
                    case BoardState.LOADED:
                    case BoardState.READY:
                    case BoardState.CONFLICT:
                    case BoardState.DONE:

                        foreach (var sn in unit.GetUnitSnSet())
                        {
                            if (state != BoardState.DONE)
                            {
                                unit.UpdateUnitResult(sn, UnitResult.FAIL, "User Abort");
                                unit.UpdateUnitState(sn, UnitState.DONE);
                            }
                            RemoveSingleUnit(boardName, sn);
                        }
                        break;
                }
                this._boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
                this._boardManager.UnsubscribeBoard(boardName);
            }, true);
        }
        public void UnitAbort(string boardName, int seat)
        {
            string sn = GetSnByPos(boardName, seat);
            if (sn == null)
                return;
            this._logger.Info("Remove Unit:" + sn);
            var unit = this._boardManager.GetBoardUnit(boardName);
            this.WaitToExecute(() =>
            {
                unit.UpdateUnitResult(sn, UnitResult.FAIL, "User Abort");
                unit.UpdateUnitState(sn, UnitState.DONE);
                RemoveSingleUnit(boardName, sn);
                if (unit.GetUnitSnSet().Any() == false)
                    this._boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
            }, true);
        }



        public void CheckAllConnection()
        {
            this._logger.Info("Start Check Connection");
            this.WaitToExecute(() =>
            {
                Parallel.ForEach(this._boardManager.SelectBoardByState(BoardState.LOADED), name =>
                {
                    this.ExceptionControl(this.CheckDutsOnBoard, name);
                });
            }, true);
        }
        public void StartBurnIn()
        {
            this._logger.Info("Start Burn In");
            this.WaitToExecute(() =>
            {
                var boardToStart = this._boardManager.SelectBoardByState(BoardState.READY);
                Parallel.ForEach(boardToStart, x =>
                {
                    SetUpTemperature(x);
                    WaitForBurnInTemperature(x);
                    SetUpBoardElectric(x);
                    StartBurnInOnBoard(x);
                });
            }, true);
        }
        public void PauseAll()
        {
            this._logger.Info("Start Pause All");
            this.WaitToExecute(() =>
            {
                var boardToPause = this._boardManager.SelectBoardByState(BoardState.RUNNING);
                Parallel.ForEach(boardToPause, x =>
                {
                    TearDownBoardElectric(x);
                    TearDownTemperature(x);
                    SetBoardPause(x);
                });
            }, true);
        }
        public void PauseBoard(string boardName)
        {
            this._logger.Info("Start Pause Board:" + boardName);
            this.WaitToExecute(() =>
            {
                var unit = this._boardManager.GetBoardUnit(boardName);
                if (unit != null)
                {
                    if (unit.GetBoardInfo().Flag == BoardState.RUNNING)
                    {
                        TearDownBoardElectric(boardName);
                        TearDownTemperature(boardName);
                        SetBoardPause(boardName);
                    }
                }

            }, true);
        }



        public EventHandler ProductsUpdate
        {
            get { return this._boardManager.ProductsUpdate; }
            set { this._boardManager.ProductsUpdate = value; }
        }

        public EventHandler BoardStateChanged
        {
            get { return this._boardManager.BoardStateChanged; }
            set { this._boardManager.BoardStateChanged = value; }
        }


    

        public void CreateScanResultFile()
        {
            //using (var sr = System.IO.File.CreateText(Utility.GetDefaultSnFile()))
            //{
            //    foreach (string sn in this._boardManager.GetAllSnSet())
            //        sr.WriteLine(sn);
            //    sr.Close();
            //}


            //foreach (string name in boardManager.SelectBoardByState(BoardState.SELECTED))
            //    foreach (string sn in unitManager.GetUnitSnSet(name))
            //        this._logger.Info(name + "\t" + sn);
        }

        private void UpdateCostTime()
        {
            List<string> doneSet = new List<string>();
            List<string> movesn = new List<string>();

            foreach (var boardName in this._boardManager.SelectBoardByState(BoardState.RUNNING))
            {
                var boardUnit = this._boardManager.GetBoardUnit(boardName);
                var result = boardUnit.GetUnitSnSet(UnitState.BURNIN);
                doneSet.AddRange(result.FindAll(sn =>
                {
                    boardUnit.UpdateCostTime(sn);
                    return boardUnit.CheckTimeOut(sn);
                }));
            }
            //if (doneSet.Count > 0)
            //{
            //    this.ScanBoardDone(doneSet);
            //    if (this._configParam.mesParam.mesCheck)
            //    {
            //        MoveNextStation(doneSet);
            //    }
            //}
        }

        private void Save_And_Check_Data(string boardName,string sn, Dictionary<int, List<KeyValuePair<string, string>>> data)
        {
            string board;
            int seat;
            var boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                boardUnit.GetBoardSeat(sn, out board, out seat);
                var dataSet = data[seat];
                try
                {
                    boardUnit.UpdateParaTime(sn);
                    boardUnit.UploadData(sn, dataSet);
                    string message = "";
                    SpecResult ret = boardUnit.CheckDataBySpec(sn, dataSet, out message);
                    bool doStop;
                    boardUnit.DealWithSpecResult(sn, ret, message, out doStop);
                    if (doStop)
                    {
                        IBoard controller = boardUnit.GetController();
                        if (controller != null)
                            controller.CatchException(seat);
                        else
                            this._logger.Warn(sn + " stop fail ");
                    }

                    //if (ret.Stopable)
                    //{
                    //    if (this._configParam.mesParam.mesCheck)
                    //    {
                    //        //Add for MES function, defause values "", if value not null, no need the MES function
                    //        if (this._configParam.mesParam.mesHoldFlag)
                    //        {
                    //            try
                    //            {
                    //                string retMsg = "";
                    //                if (this._mesOperator.Hold(sn, "NG Auto Hold", out retMsg))
                    //                {
                    //                    this._logger.Info("The " + sn + " Auto Hold in this station.");
                    //                }
                    //                else
                    //                {
                    //                    this._logger.Warn(sn + " Fail to Hold.Reason is " + retMsg);
                    //                }
                    //            }
                    //            catch
                    //            {
                    //                this._logger.Warn(sn + " Fail to Hold for MES reason.");
                    //            };
                    //        }
                    //        else
                    //        {
                    //            this._logger.Info("The " + sn + " user no need to hold.");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        this._logger.Info("The " + sn + " no need to check MES Action.");
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    this._logger.Error(sn + ":" + ex.Message);
                }
            }
            
            
        }

        private void UpdateDutsState(List<string> snSet, UnitState nextState)
        {
            //foreach (string sn in snSet)
            //    unitManager.UpdateUnitState(sn, nextState);
        }

        private void RaiseProductsUpdate()
        {
            this.ProductsUpdate?.Invoke(this, null);
        }

        private void WaitForBurnInTemperature(string boardName = "")
        {
            this._logger.Info(boardName + " " + "Wait For Burn-In Temperature...");
            int heatTime = this._configParam.systemParam.heatTime;//int.Parse(SettingsReader.GetInstance().GetSettingsValue("OtherSetting", "HEATTIME"));
            while (heatTime > 0)
            {
                this._logger.Info(boardName + " " + "Remain:\t" + heatTime + " min");
                if (heatTime > 1)
                    Thread.Sleep(60000);
                else
                    for (int i = 59; i >= 0; i--)
                    {
                        Thread.Sleep(1000);
                        if (i % 10 == 0 || i <= 5)
                            this._logger.Info(boardName + " " + "Remain:\t" + i + " sec");
                    }
                heatTime--;
            }
        }

        private void RemoveSingleUnit(string boardName, string sn)
        {
            var unit = this._boardManager.GetBoardUnit(boardName);
            if (unit == null)
            {
                return;
            }
            string board;
            int seat;
            unit.GetBoardSeat(sn, out board, out seat);
            this._boardManager.RemoveSeatFromBoard(board, seat);

        }

        private void ExceptionControl(BoardAction action, string boardName)
        {
            try
            {
                action(boardName);
            }
            catch (Exception ex)
            {
                this._logger.Error(boardName + ":" + ex.Message);
                this._logger.Error(boardName + ex.StackTrace);
                this._boardManager.ChangeBoardState(boardName, BoardState.CONFLICT);
            }
        }


        public List<string> GetSelectedBoard()
        {
            return this._boardManager.SelectBoardByState(BoardState.SELECTED);
        }
        public int GetSeatsCount(string boardName)
        {
            var boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
                return boardUnit.GetController().SeatsCount;
            else
                return 0;
        }
        public Dictionary<int, string> GetSnSet(string boardName)
        {
            var boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
                return boardUnit.GetController().GetSnSet();
            else
                return null;
        }
      
        private void _BindingSnOnBoard(string boardName, Dictionary<int, string> snSet)
        {
            if (snSet != null && snSet.Count != 0)
            {
                BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
                if (boardUnit != null)
                {
                    double Temp =boardUnit.GetController().GetTargetOvenTemperature();
                    foreach (var seat in snSet.Keys)
                    {
                        string sn = snSet[seat];
                        this._boardManager.AddSeatToBoard(boardName, seat, sn);

                    }
                    if (boardUnit.GetUnitSnSet().Any() == false)
                    {
                        this._logger.Warn(boardName + " is Empty.");
                        this._boardManager.ChangeBoardState(boardName, BoardState.UNSELECTED);
                        this._boardManager.UnsubscribeBoard(boardName);
                    }
                    else
                    {
                        this._boardManager.ChangeBoardState(boardName, BoardState.LOADED);
                    }
                } 
            }
            
        }
        public void BindingSnOnBoard(string boardName, Dictionary<int, string> snSet)
        {
            WaitToExecute(() =>
            {
                ExceptionControl(delegate (string board)
                {
                    _BindingSnOnBoard(board, snSet);

                }, boardName);
            }, true);
        }

        private void CheckDutsOnBoard(string boardName)
        {
            bool checkPass = true;
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                var result = boardUnit.GetController().CheckConnections();
                List<string> failInfoSet = new List<string>();
                foreach (int seat in result.Keys)
                {
                    if (result[seat] == false)
                    {
                        checkPass = false;
                        failInfoSet.Add(boardUnit.GetSn(seat));
                    }
                }
                if (checkPass == false)
                    this.UpdateDutsState(failInfoSet, UnitState.REWORK);
                else
                    this.UpdateDutsState(boardUnit.GetUnitSnSet(UnitState.REWORK), UnitState.READY);
                this._boardManager.ChangeBoardState(boardName, checkPass ? BoardState.READY : BoardState.LOADED);
                this._logger.Info(boardName + ":" + (checkPass ? " PASS" : " FAIL"));
            }
        }

        private void StartBurnInOnBoard(string boardName)
        {
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                this.UpdateDutsState(boardUnit.GetUnitSnSet(), UnitState.BURNIN);
            }
            this._boardManager.ChangeBoardState(boardName, BoardState.RUNNING);
        }

        private void SetBoardPause(string boardName)
        {
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                var locateSet = boardUnit.GetUnitSnSet();
                this.UpdateDutsState(locateSet, UnitState.PAUSE);
            }
            this._boardManager.ChangeBoardState(boardName, BoardState.LOADED);
        }

        private void ReadDataOnBoard(string boardName)
        {
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                BoardInfo info = boardUnit.GetBoardInfo();
                List<string> readList = boardUnit.GetUnitSnSet(UnitState.BURNIN).FindAll(sn => boardUnit.ReadActive(sn));
                if (readList.Count > 0)
                {
                    try
                    {
                        var data = boardUnit.GetController().ReadDataSet("RUN");
                        
                        info.ErrorCount = 0;
                        foreach (string sn in readList)
                            this.Save_And_Check_Data(boardName,sn, data);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(boardName + ":" + ex.Message);
                        info.ErrorCount += 1;
                        if (info.ErrorCount > this._configParam.systemParam.comErrorTolarence)
                            throw new Exception("Above Com Error Tolarence");
                    }
                }
            }


           
            
        }

        private bool SetUpTemperature(string boardName)
        {
            bool ret = false; 
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                double temperature = boardUnit.GetController().GetTargetOvenTemperature();
                ret = boardUnit.GetController().SetUpTemperature(temperature);
                this._logger.Info(boardName + " Setup Temperature " + (ret ? "Arrived." : "Unarrived."));
            }
            return ret;
        }

        private bool TearDownTemperature(string boardName)
        {
            bool ret = false;
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                ret = boardUnit.GetController().TearDownTemperature();
                this._logger.Info(boardName + " TearDown Temperature " + (ret ? "Arrived." : "Unarrived."));
            }
                 
            return ret;
        }

        private bool SetUpBoardElectric(string boardName)
        {
            bool ret = false;
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                ret = boardUnit.GetController().EnableBoard();
                this._logger.Info(boardName + " SetUp Electric " + (ret ? "Finished." : "Unfinished."));
            }
            
            return ret;
        }

        private bool TearDownBoardElectric(string boardName)
        {
            bool ret = false;
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                ret = boardUnit.GetController().DisableBoard();
                this._logger.Info(boardName + " TearDown Electric " + (ret ? "Finished." : "Unfinished."));
            }    
            return ret;
        }



        public string[] GetUnitParaSet(string boardName,string sn)
        {
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                return boardUnit.GetUnitParaSet(sn);
            }
            return null;      
        }

        public DataTable FetchUnitData(string boardName, string sn, int range = 200)
        {
            BoardUnit boardUnit = this._boardManager.GetBoardUnit(boardName);
            if (boardUnit != null)
            {
                return boardUnit.FetchUnitData(sn, range);
            }
            return null;
            
        }

        private bool MoveNextStation(List<string> dataList)
        {
            bool flag = true;
            if ((dataList.Count > 0))
            {
                foreach (string locate in dataList)
                {
                    string sn = locate;
                  
                    //if (this._mesOperator.AutoMoveOut(sn, unitManager.GetUnitDetail(sn)["UnitResult"], this._configParam.mesParam.mesHoldFlag, out holdormove))
                    //{
                    //    //This Pass SN normally Move next station
                    //    if (holdormove == 1)
                    //    {
                    //        this._logger.Info("The " + sn + " Auto moved next test station.");
                    //    }
                    //    //The Fail SN normally Hold on this station
                    //    if (holdormove == 2)
                    //    {
                    //        this._logger.Info("The " + sn + " Auto Hold in this station.");
                    //    }
                    //    //For the Failed SN, user no need to hold
                    //    if (holdormove == 3)
                    //    {
                    //        this._logger.Info("The " + sn + " No need to hold in this station.");
                    //    }

                    //}
                    //else
                    //{
                    //    //Call the SN MES move or hold function abnormal
                    //    this._logger.Info("The " + sn + " Failed to move next test station!");
                    //    flag = false;
                    //}
                }
            }
            return flag;
        }

    }
}


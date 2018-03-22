using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BILib;
using TbiesIntfDll;
using BIModel.Data;
using log4net;
namespace BIModel.Manager
{
    class UnitManager
    {
        private List<Guid> _runList;
        private List<UnitEntry> _unitSet;
        private IDatabaseService dataService;
        private ILog Log;
        private SystemParams _param;
        public EventHandler ProductsUpdate { get; set; }

        private static string _targetFile = Path.Combine(Utility.RunTimeDirectory, @"UnitRuntime.xml");
        public UnitManager(ILog logger, SystemParams param)
        {
            this.Log = logger;
            this._param = param;
            Utility.Load(_targetFile, out _runList);
            if (_runList == null)
                _runList = new List<Guid>();
            _unitSet = new List<UnitEntry>();

        }

        public void SyncFromStore(IDatabaseService dataService)
        {
            this.dataService = dataService;
            foreach (var item in _runList)
            {
                string target = Path.Combine(Utility.RunTimeDirectory, item.ToString());
                UnitEntry entry = null;
                Utility.Load(target, out entry);
                _unitSet.Add(entry);
            }
            ProductsUpdate?.Invoke(this, null);
        }

        public string GetSn(string name, int seat)
        {
            var result = from x in this._unitSet where x.Board == name && x.Seat == seat select x.Sn;
            return result.Any() ? result.First() : null;
        }

        private UnitEntry GetUnitInfo(string sn)
        {
            var result = from x in this._unitSet where x.Sn == sn select x;
            return result.Any() ? result.First() : null;
        }

        public List<string> GetUnitSnSet(string board)
        {
            return (from x in _unitSet where x.Board == board select x.Sn).ToList();
        }

        public List<string> GetUnitSnSet(string board, UnitState state)
        {
            return (from x in _unitSet where x.Board == board && x.State == state select x.Sn).ToList();
        }

        public List<string> GetAllSnSet()
        {
            return (from x in _unitSet select x.Sn).ToList();
        }

        public void UnsubscribeUnit(string sn)
        {
            this.Log.Info("[Unit Pool]\t>>>>\t" + sn);
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = "+ sn +":unit info is not exist in unit set,unsubscrible unit fail");
                return;
            }

            dataService.UpdateBiReocordData(info.Id, info.FinishTime, info.Cost.ToString("F2"), info.Result.ToString(),
                info.Comment);
            _unitSet.RemoveAll(x => x.Sn == sn);
            _runList.RemoveAll(x => x.ToString() == info.Id.ToString());
            Utility.Dump(_targetFile, _runList);
            string entryTarget = Path.Combine(Utility.RunTimeDirectory, info.Id.ToString());
            File.Delete(entryTarget);
            ProductsUpdate?.Invoke(this, null);
        }

        public void SubscribeUnit(string sn, string plan, double ovenTemperature, string board, int seat)
        {

            if (_unitSet.FindAll(x => x.Sn == sn).Any())
                throw new Exception(sn + " exists in current system.");
            this.Log.Info(sn + "\t>>>>\t[Unit Pool]");

            var driverType = FetchPlans.Inst(this._param.tbiesServer).GetDriverType(plan);
            Guid id = dataService.CreateBiRecord(sn, driverType,plan,board,seat.ToString());
            var entry = new UnitEntry()
            {
                Id = id,
                Sn = sn,
                Plan = plan,
                Board = board,
                Seat = seat,
                Cost = 0,
                Comment = "",
                State = UnitState.READY,
                Result = UnitResult.PASS,
                CreateTime = DateTime.Now,
                CostCounter = DateTime.Now,
                ReadCounter = DateTime.Now,
                FinishTime = DateTime.Now,
                OutSpec = false,
                OutStart = DateTime.Now,
            };
            _unitSet.Add(entry);
            DumpEntry(entry);
            _runList.Add(id);
            Utility.Dump(_targetFile, _runList);
            ProductsUpdate?.Invoke(this, null);
        }

        public void UpdateStartBurnTime(string board)
        {
            var snSet = GetUnitSnSet(board);
            var startTime = DateTime.Now;
            foreach (var sn in snSet)
            {
                var unit = GetUnitInfo(sn);
                dataService.UpdateBiReocordData(unit.Id, startTime);
            }
        }

        private double GetTimeOut(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
               // this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,get timeOut  fail");
                return 0;
            }
            double timeOut = FetchPlans.Inst(this._param.tbiesServer).GetSpan(info.Plan) * 60;
            return timeOut;

        }

        public int GetProgress(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,get progress  fail");
                return 0;  
            }    
            double timeOut = GetTimeOut(sn);
            double costTime = info.Cost;
            var val = costTime * 100 / timeOut;
            return (int)(val > 100 ? 100 : val);
        }

        public Dictionary<string, string> GetUnitDetail(string sn)
        {
            var ret = new Dictionary<string, string>();
            var info = GetUnitInfo(sn);

            double timeOut = GetTimeOut(sn);
            if (info != null)
            {
                ret.Add("SerialNumber", info.Sn);
                ret.Add("Plan", info.Plan);
                ret.Add("Board", info.Board);
                ret.Add("Seat", info.Seat.ToString());
                ret.Add("CostTime", info.Cost.ToString("F2"));
                ret.Add("RemainTime", (timeOut - info.Cost).ToString("F2"));
                ret.Add("UnitState", info.State.ToString());
                ret.Add("UnitResult", info.Result.ToString());
                ret.Add("CreateTime", info.CreateTime.ToString());
                ret.Add("MonitorTime", info.ReadCounter.ToString());
            }
            return ret;
        }

        public void UpdateUnitState(string sn, UnitState state)
        {
            Log.Info(sn + "\t--->\t" + state);
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,update unit state  fail");
                return ;
            }
            info.State = state;
            info.CostCounter = DateTime.Now;
            info.FinishTime = DateTime.Now;
            DumpEntry(info);
            ProductsUpdate?.Invoke(this, null);
        }

        public void UpdateUnitResult(string sn, UnitResult result, string comment)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,update unit result  fail");
                return;
            }
            info.Result = result;
            info.Comment = comment;
            info.FinishTime = DateTime.Now;
            info.Result = result;
            DumpEntry(info);
            ProductsUpdate?.Invoke(this, null);
        }

        public void UpdateCostTime(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,update cost time  fail");
                return;
            }
            info.Cost += info.OutSpec ? 0 : DateTime.Now.Subtract(info.CostCounter).TotalMinutes;
            info.CostCounter = DateTime.Now;
            info.FinishTime = DateTime.Now;
            DumpEntry(info);
            ProductsUpdate?.Invoke(this, null);
        }

        public bool CheckTimeOut(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,check time out fail");
                return false;
                
            }
            double timeOut = GetTimeOut(sn);
            return info.Cost >= timeOut;
        }

        public void UpdateParaTime(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,update para time fail");
                return  ;

            }
            info.ReadCounter = DateTime.Now;
            DumpEntry(info);
        }

        public bool ReadActive(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,read active fail");
                return false;

            }
            double interval = FetchPlans.Inst(this._param.tbiesServer).GetInterval(info.Plan);
            return DateTime.Now.Subtract(info.ReadCounter).TotalMinutes >= interval;
        }

        public SpecResult CheckDataBySpec(string sn, List<KeyValuePair<string, string>> data, out string message)
        {
            message = "";
            var ret = new SpecResult { CompareFail = false, Pausable = false, Stopable = false };
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,check data by spec fail");
                return ret;

            }
            SpecItem[] specArray = FetchPlans.Inst(this._param.tbiesServer).GetSpecification(info.Plan);
            List<SpecItem> failList = new List<SpecItem>();
            
            foreach (var specItem in specArray)
                foreach (var dataRow in data)
                    if (dataRow.Key == specItem.Item)
                        if (!(double.Parse(specItem.LBound) <= double.Parse(dataRow.Value) && double.Parse(dataRow.Value) < double.Parse(specItem.UBound)))
                        {
                            message += specItem.Item;
                            failList.Add(specItem);
                        }
           
            foreach (var spec in failList)
            {
                if (spec.Type.Contains("C"))
                    ret.CompareFail = true;
                if (spec.Type.Contains("P"))
                    ret.Pausable = true;
                if (spec.Type.Contains("S"))
                    ret.Stopable = true;
            }
            return ret;
        }

        private void DumpEntry(UnitEntry entry)
        {
            string entryTarget = Path.Combine(Utility.RunTimeDirectory, entry.Id.ToString());
            Utility.Dump(entryTarget, entry);
        }

        public string GetUnitStyle(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                return "EMPTY";

            }
            return (UnitResult.FAIL == info.Result || UnitState.DONE == info.State) ? info.Result.ToString() : info.State.ToString();
        }

        public void GetBoardSeat(string sn, out string board, out int seat)
        {
            board = "";
            seat = 0;
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,get board seat fail");
                return;
            }
            board = info.Board;
            seat = info.Seat;
        }

        public void DealWithSpecResult(string sn, SpecResult specResult, string message, out bool doStop)
        {
            doStop = false;
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,deal with spec result fail");
                return;
            }
           
            if (specResult.CompareFail)
            {
                this.Log.Warn(sn + " COMP PARA FAIL:" + message);
                UpdateUnitResult(sn, UnitResult.FAIL, "Out OF SPEC:" + message);
            }
            if (specResult.Stopable)
            {
                this.Log.Warn(sn + " KEY PARA FAIL:" + message);
                this.Log.Warn(sn + " Try to Stop.");
                UpdateUnitResult(sn, UnitResult.FAIL, "Out OF SPEC:" + message);
                UpdateUnitState(sn, UnitState.DONE);
                doStop = true;
            }
            if (specResult.Pausable)
            {
                this.Log.Warn(sn + " COND PARA FAIL:" + message);
                if (info.OutSpec == true)
                {
                    double timeOut = this._param.conditionTimeout;// double.Parse(SettingsReader.GetInstance().GetSettingsValue("OtherSetting", "Condition_Timeout"));
                    if (DateTime.Now.Subtract(info.OutStart).TotalMinutes > timeOut)
                    {
                        UpdateUnitState(sn, UnitState.PAUSE);
                        doStop = true;
                    }
                }
                else
                {
                    info.OutSpec = true;
                    info.OutStart = DateTime.Now;
                }
            }
            else
            {
                info.OutSpec = false;
            }
        }

        public void UploadData(string sn, List<KeyValuePair<string, string>> data)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,upload data fail");
                return;
            }
            dataService.InsertBiData(info.Id,data);
        }

        public string[] GetUnitParaSet(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,get unit para set fail");
                return null;
            }
            return dataService.GetBiDataItems(info.Id);
        }

        public DataTable FetchUnitData(string sn, int range)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this.Log.Error("sn = " + sn + ":unit info is not exist in unit set,fetch unit data fail");
                return null;
            }
            return dataService.GetBiData(info.Id, range);
        }


        //private static MesServiceClient mMesClient = new MesServiceClient();
        //private static void ServiceInvoke(Func<string[], bool> func)
        //{
        //    string[] str = { "" };
        //    if (func(str) == false)
        //        throw new Exception(str[0]);
        //}
        //public static void StartMes()
        //{
        //    MesServiceCtrl.Start();
        //}
        //public static void StopMes()
        //{
        //    MesServiceCtrl.Stop();
        //}
    }
        
}

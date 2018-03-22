using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BIModel.Interface;
using BILib;
using BIModel.Manager;
using System.Data;
using BIModel.Data;
using TbiesIntfDll;
using BiInterface;
using log4net;
namespace BIModel.NowCoding
{
    public class BoardUnit
    {
        private IBoard _Controller;
        private BoardInfo _boardInfo;
        private ILog _logger;
        private SystemParams _param;
        private IDatabaseService _dataService;
        public BoardUnit(ILog logger, IDatabaseService dataService, SystemParams param)
        {
            this._boardInfo = new BoardInfo();
            this._logger = logger;
            this._param = param;
            this._dataService = dataService;
        }
        static public EventHandler ProductsUpdate { get; set; }
    
        public IBoard GetController()
        {
            return this._Controller;
        }
        public void SetController(IBoard controller)
        {
            this._Controller = controller;
        }
        
        public BoardInfo GetBoardInfo()
        {
            return this._boardInfo;
        }
        public void SetBoardInfo(BoardInfo info)
        {
            this._boardInfo.BoardName = info.BoardName;
            this._boardInfo.Plan = info.Plan;
            this._boardInfo.Flag = info.Flag;
            this._boardInfo.ErrorCount = info.ErrorCount;
            this._boardInfo.units = info.units;
            this._boardInfo.temperature = info.temperature;
        }
     
        public string GetSn(int seat)
        {
            var result = from x in this._boardInfo.units where x.Seat == seat select x.Sn;
            return result.Any() ? result.First() : null;
        }
        public UnitInfo GetUnitInfo(string sn)
        {
            var result = from x in this._boardInfo.units where x.Sn == sn select x;
            return result.Any() ? result.First() : null;
        }
        public List<string> GetUnitSnSet()
        {
            return (from x in this._boardInfo.units select x.Sn).ToList();
        }
        public List<string> GetUnitSnSet(UnitState state)
        {
            return (from x in this._boardInfo.units where x.State == state select x.Sn).ToList();
        }
       
        private double GetTimeOut(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,get timeout fail");
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
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,get progress  fail");
                return 0;
            }
            double timeOut = GetTimeOut(sn);
            double costTime = info.Cost;
            var val = costTime * 100 / timeOut;
            return (int)(val > 100 ? 100 : val);
        }

        public Dictionary<string, string> GetUnitDetail(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,get unit detail fail");
                return null;
            }
            var ret = new Dictionary<string, string>();
            
            double timeOut = GetTimeOut(sn);
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
            return ret;
        }

        public void UpdateUnitState(string sn, UnitState state)
        {
            this._logger.Info(sn + "\t--->\t" + state);
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,update unit state  fail");
                return;
            }
            info.State = state;
            info.CostCounter = DateTime.Now;
            info.FinishTime = DateTime.Now;
            ProductsUpdate?.Invoke(this, null);
        }

        public void UpdateUnitResult(string sn, UnitResult result, string comment)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,update unit result  fail");
                return;
            }
            info.Result = result;
            info.Comment = comment;
            info.FinishTime = DateTime.Now;
            info.Result = result;
            ProductsUpdate?.Invoke(this, null);
        }

        public void UpdateCostTime(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,update cost time  fail");
                return;
            }
            info.Cost += info.OutSpec ? 0 : DateTime.Now.Subtract(info.CostCounter).TotalMinutes;
            info.CostCounter = DateTime.Now;
            info.FinishTime = DateTime.Now;
          
            ProductsUpdate?.Invoke(this, null);
        }

        public bool CheckTimeOut(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,check time out fail");
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
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,update para time fail");
                return;

            }
            info.ReadCounter = DateTime.Now;
           
            ProductsUpdate?.Invoke(this, null);
        }

        public bool ReadActive(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,read active fail");
                return false;

            }
            double interval = FetchPlans.Inst(this._param.tbiesServer).GetInterval(info.Plan);
            return DateTime.Now.Subtract(info.ReadCounter).TotalMinutes >= interval;
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
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,get board seat fail");
                return;
            }
            board = info.Board;
            seat = info.Seat;
        }
        
        public void UploadData(string sn, List<KeyValuePair<string, string>> data)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,upload data fail");
                return;
            }
            this._dataService.InsertBiData(info.Id, data);
        }

        public string[] GetUnitParaSet(string sn)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,get unit para set fail");
                return null;
            }
            return this._dataService.GetBiDataItems(info.Id);
        }

        public DataTable FetchUnitData(string sn, int range)
        {
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,fetch unit data fail");
                return null;
            }
            return this._dataService.GetBiData(info.Id, range);
        }


        public SpecResult CheckDataBySpec(string sn, List<KeyValuePair<string, string>> data, out string message)
        {
            message = "";
            var ret = new SpecResult { CompareFail = false, Pausable = false, Stopable = false };
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,check data by spec fail");
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
        public void DealWithSpecResult(string sn, SpecResult specResult, string message, out bool doStop)
        {
            doStop = false;
            var info = GetUnitInfo(sn);
            if (info == null)
            {
                this._logger.Error("sn = " + sn + ":unit info is not exist in unit set,deal with spec result fail");
                return;
            }

            if (specResult.CompareFail)
            {
                this._logger.Warn(sn + " COMP PARA FAIL:" + message);
                UpdateUnitResult(sn, UnitResult.FAIL, "Out OF SPEC:" + message);
            }
            if (specResult.Stopable)
            {
                this._logger.Warn(sn + " KEY PARA FAIL:" + message);
                this._logger.Warn(sn + " Try to Stop.");
                UpdateUnitResult(sn, UnitResult.FAIL, "Out OF SPEC:" + message);
                UpdateUnitState(sn, UnitState.DONE);
                doStop = true;
            }
            if (specResult.Pausable)
            {
                this._logger.Warn(sn + " COND PARA FAIL:" + message);
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
    }
}

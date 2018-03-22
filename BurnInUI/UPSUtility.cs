using System;
using System.Timers;
using BurnInUI.ConfigReader;
using log4net;

namespace BurnInUI
{
    public class UPSUtility
    {
        private  ILog _logger;
        private  Timer _UpsMonitor;
        private  DateTime _WarningStart;
        private  int _WarningTimeOut;
        private  StrComm _com;
        private static UPSUtility upsUtility;
        public EventHandler UpsWarningTimeOut;
        public static UPSUtility Instance(ILog logger,ConfigInfo param)
        {
            if (upsUtility == null)
            {
                upsUtility = new UPSUtility(logger,param);
            }
                
            return upsUtility;
        }
        private UPSUtility(ILog logger,ConfigInfo param)
        {
            this._logger = logger;
            this._UpsMonitor = new Timer();
            this._UpsMonitor.Interval = 5000;
            this._UpsMonitor.Elapsed += UpsMonitorTick;
            this._WarningStart = DateTime.Now;
            this._WarningTimeOut = param.UpsTimeOut;
            this._com = new StrComm(param.UpsPort, 2400);
        }
        private  bool CheckUpsCommunication()
        {
            try
            {
                this._com.Query("Y\r\n",100);
                return true;
            }
            catch(Exception ex)
            {
                this._logger.Error("Check UPS Comm:"+ex.Message);
                return false;
            }
        }
        
        private  bool CheckUpsWarning()
        {
            string rep = this._com.Query("Y\r\n",100);
            if (rep.StartsWith("SM"))
                rep = this._com.Query("Q\r\n",100);
            return rep.StartsWith("10");
        }

        private  void UpsMonitorTick(object obj, ElapsedEventArgs args)
        {
            try
            {
                this._UpsMonitor.Stop();
                if (CheckUpsWarning())
                {
                    this._logger.Warn("UPS is Working, please check power supply!!!");
                    TimeSpan timeSpan = DateTime.Now.Subtract(this._WarningStart);
                    if (timeSpan.Minutes > this._WarningTimeOut)
                        UpsWarningTimeOut(null, null);
                }
                else
                {
                    this._WarningStart = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                this._logger.Error("UPS:" + ex.Message);
            }
            finally
            {
                this._UpsMonitor.Start();
            }
        }
        public  void StartUPS()
        {
            try
            {
                bool result = CheckUpsCommunication();
                if (result)
                {
                    this._logger.Info("Check UPS Communication PASS.");
                    this._UpsMonitor.Start();
                }
                else
                {
                    this._logger.Info("Check UPS Communication FAIL.");
                    this._logger.Info("Fail to Open UPS Service.");
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message);
            }
        }


    }
}

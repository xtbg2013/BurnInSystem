using System;
using System.Collections.Generic;
using log4net;

namespace BurnInUI.ConfigReader
{
    public class ConfigInfo
    {
        public string SqlScript { set; get; }
        public string DataSource { set; get; }
        public string DataBaseName { set; get; }
        public string UserId { set; get; }
        public string Password { set; get; }

        public string LocalBiDataSummaryTable { set; get; }
        public string LocalBiDataTable { set; get; }
      

        public string RemoteSqlScript { set; get; }
        public string RemoteDataSource { set; get; }
        public string RemoteDataBaseName { set; get; }
        public string RemoteUserId { set; get; }
        public string RemotePassword { set; get; }
        public string RemoteSummaryBiDataTable { set; get; }
        public string RemoteBiDataTable { set; get; }

        public string TestStation { set; get; }
        public string MbPath { set; get; }
        public int HeatTime { set; get; }
        public int ComErrorTolarence  { set; get; }
        public int ConditionTimeout { set; get; }
        public int FloorCount { set; get; }
        public string OvenPort { set; get; }
        public string Floor1Port { set; get; }
        public string Floor2Port { set; get; }
        public string Floor3Port { set; get; }
        public string Floor4Port { set; get; }
        public string Floor5Port { set; get; }
        public string Floor6Port { set; get; }
        public string Floor7Port { set; get; }
        public string Floor8Port { set; get; }
        public string Floor9Port { set; get; }
        public string Floor10Port { set; get; }
        public string MesServiceUrl { set; get; }
 
        public string UpsSwitch { set; get; }
        public int UpsTimeOut { set; get; }
        public string UpsPort { set; get; }
        public string TbiesServer { set; get; }
        public int BakupPeriodMinutes { set; get; }
        public int DeletePeriodDays { set; get; }
    }
    public class LoadConfig
    {
        private readonly ILog _logger;
        private readonly ConfigReader _reader;
        public LoadConfig(ILog logger)
        {
            _logger = logger;
            try
            {
                _reader = ConfigReader.GetInstance(@"configurations\Settings.xml");
            }
            catch(Exception ex)
            {
                _logger.Error("Load config file exception:" + ex.Message);
            }
        }
        public ConfigInfo LoadConfigParam()
        {
            var info = new ConfigInfo();
            try
            {
                var param = LoadXml();
                info.SqlScript = param["SqlScript"];
                info.DataSource = param["DataSource"]; 
               
                info.DataBaseName = param["DatabaseName"]; 
                info.UserId = param["UserId"]; 
                info.Password = param["Password"];
                info.LocalBiDataSummaryTable = param["LocalBiDataSummaryTable"];
                info.LocalBiDataTable = param["LocalBiDataTable"];
               
                info.RemoteSqlScript = param["RemoteSqlScript"]; 
                info.RemoteDataSource = param["RemoteDataSource"]; 
                info.RemoteDataBaseName = param["RemoteDatabaseName"]; 
                info.RemoteUserId = param["RemoteUserId"]; 
                info.RemotePassword = param["RemotePassword"]; 
                info.RemoteSummaryBiDataTable = param["RemoteSummayBiDataTable"]; 
                info.RemoteBiDataTable = param["RemoteBiDataTable"];
             

                info.TestStation = Environment.MachineName;
                info.MbPath = param["MBTPATH"]; 
                info.HeatTime = int.Parse(param["HEATTIME"]); 
                info.ComErrorTolarence = int.Parse(param["COM_ERROR_TOLARENCE"]); 
                info.ConditionTimeout = int.Parse(param["Condition_Timeout"]); 
                info.FloorCount = int.Parse(param["FloorCount"]); 
                info.OvenPort = param["OVEN_PORT"]; 
            
                info.Floor1Port = param["Floor1"]; 
                info.Floor2Port = param["Floor2"]; 
                info.Floor3Port = param["Floor3"]; 
                info.Floor4Port = param["Floor4"]; 
                info.Floor5Port = param["Floor5"]; 
                info.Floor6Port = param["Floor6"]; 
                info.Floor7Port = param["Floor7"]; 
                info.Floor8Port = param["Floor8"]; 
                info.Floor9Port = param["Floor9"]; 
                info.Floor10Port = param["Floor10"]; 
                info.TbiesServer = param["TbiesServer"]; 

                info.MesServiceUrl = param["MES_SERVICE"]; 
                info.UpsSwitch = param["SWITCH"]; 
                info.UpsTimeOut = int.Parse(param["TIMEOUT"]); 
                info.UpsPort = param["PORTNAME"];
                info.BakupPeriodMinutes = int.Parse(param["BakupPeriodMinutes"]);
                info.DeletePeriodDays = int.Parse(param["DeletePeriodDays"]);

            }
            catch (Exception ex)
            {
                _logger.Error("Load config exception:" + ex.Message);
            }
            return info;

        }

        public Dictionary<string, string> LoadXml()
        {
            try
            {
                var xmlInfo = new Dictionary<string, string>
                {
                    ["DataSource"] = string.Format(_reader.GetItem("OtherSetting", "DataSource"), Environment.MachineName),
                    ["DatabaseName"] = _reader.GetItem("OtherSetting", "DatabaseName"),
                    ["UserId"] = "sa",
                    ["Password"] = "cml@shg629",
                    ["SqlScript"] = @"SqlScript\BMS37.sql",

                    ["LocalBiDataSummaryTable"] = _reader.GetItem("OtherSetting", "LocalBiDataSummaryTable"),
                    ["LocalBiDataTable"] = _reader.GetItem("OtherSetting", "LocalBiDataTable"),



                    ["RemoteSqlScript"] = @"SqlScript\RemoteBms37.sql",
                    ["RemoteDataSource"] = _reader.GetItem("OtherSetting", "RemoteDataSource"),
                    ["RemoteDatabaseName"] = _reader.GetItem("OtherSetting", "RemoteDatabaseName"),

                    ["RemoteUserId"] = "BI_Data_Admin",
                    ["RemotePassword"] = "BI_Data_Admin@123",

                    ["RemoteSummayBiDataTable"] = _reader.GetItem("OtherSetting", "RemoteSummayBiDataTable"),
                    ["RemoteBiDataTable"] = _reader.GetItem("OtherSetting", "RemoteBiDataTable"),

                    ["BakupPeriodMinutes"] = _reader.GetItem("OtherSetting", "BakupPeriodMinutes"),
                    ["DeletePeriodDays"] = _reader.GetItem("OtherSetting", "DeletePeriodDays"),

                    ["MBTPATH"] = _reader.GetItem("OtherSetting", "MBTPATH"),
                    ["HEATTIME"] = _reader.GetItem("OtherSetting", "HEATTIME"),
                    ["COM_ERROR_TOLARENCE"] = _reader.GetItem("OtherSetting", "COM_ERROR_TOLARENCE"),
                    ["Condition_Timeout"] = _reader.GetItem("OtherSetting", "Condition_Timeout"),
                    ["FloorCount"] = _reader.GetItem("OtherSetting", "FloorCount"),
                    ["OVEN_PORT"] = _reader.GetItem("OtherSetting", "OVEN_PORT"),
                    ["Floor1"] = _reader.GetItem("OtherSetting", "Floor1"),
                    ["Floor2"] = _reader.GetItem("OtherSetting", "Floor2"),
                    ["Floor3"] = _reader.GetItem("OtherSetting", "Floor3"),
                    ["Floor4"] = _reader.GetItem("OtherSetting", "Floor4"),
                    ["Floor5"] = _reader.GetItem("OtherSetting", "Floor5"),
                    ["Floor6"] = _reader.GetItem("OtherSetting", "Floor6"),
                    ["Floor7"] = _reader.GetItem("OtherSetting", "Floor7"),
                    ["Floor8"] = _reader.GetItem("OtherSetting", "Floor8"),
                    ["Floor9"] = _reader.GetItem("OtherSetting", "Floor9"),
                    ["Floor10"] = _reader.GetItem("OtherSetting", "Floor10"),
                    ["TbiesServer"] = _reader.GetItem("OtherSetting", "TbiesServer"),
                    ["MES_SERVICE"] = _reader.GetItem("MES", "MES_SERVICE"),
                    ["SWITCH"] = _reader.GetItem("UPS", "SWITCH"),
                    ["TIMEOUT"] = _reader.GetItem("UPS", "TIMEOUT"),
                    ["PORTNAME"] = _reader.GetItem("UPS", "PORTNAME")
                    
                };
                
                return xmlInfo;
            }
            catch (Exception ex)
            {
                _logger.Error("Load config exception:" + ex.Message);
                return null;
            }
             
        }

        public Dictionary<string, string> LoadXmlForConfigUi()
        {
            try
            {
                var xmlInfo = new Dictionary<string, string>
                {
                    ["MBTPATH"] = _reader.GetItem("OtherSetting", "MBTPATH"),
                    ["HEATTIME"] = _reader.GetItem("OtherSetting", "HEATTIME"),
                    ["COM_ERROR_TOLARENCE"] = _reader.GetItem("OtherSetting", "COM_ERROR_TOLARENCE"),
                    ["Condition_Timeout"] = _reader.GetItem("OtherSetting", "Condition_Timeout"),
                    ["FloorCount"] = _reader.GetItem("OtherSetting", "FloorCount"),
                    ["OVEN_PORT"] = _reader.GetItem("OtherSetting", "OVEN_PORT"),
                    ["Floor1"] = _reader.GetItem("OtherSetting", "Floor1"),
                    ["Floor2"] = _reader.GetItem("OtherSetting", "Floor2"),
                    ["Floor3"] = _reader.GetItem("OtherSetting", "Floor3"),
                    ["Floor4"] = _reader.GetItem("OtherSetting", "Floor4"),
                    ["Floor5"] = _reader.GetItem("OtherSetting", "Floor5"),
                    ["Floor6"] = _reader.GetItem("OtherSetting", "Floor6"),
                    ["Floor7"] = _reader.GetItem("OtherSetting", "Floor7"),
                    ["Floor8"] = _reader.GetItem("OtherSetting", "Floor8"),
                    ["Floor9"] = _reader.GetItem("OtherSetting", "Floor9"),
                    ["Floor10"] = _reader.GetItem("OtherSetting", "Floor10"),
                    ["TbiesServer"] = _reader.GetItem("OtherSetting", "TbiesServer"),
                    ["MES_SERVICE"] = _reader.GetItem("MES", "MES_SERVICE"),
                    ["SWITCH"] = _reader.GetItem("UPS", "SWITCH"),
                    ["TIMEOUT"] = _reader.GetItem("UPS", "TIMEOUT"),
                    ["PORTNAME"] = _reader.GetItem("UPS", "PORTNAME")
                };

                return xmlInfo;
            }
            catch (Exception ex)
            {
                _logger.Error("Load config exception:" + ex.Message);
                return null;
            }

        }
        public void Save(Dictionary<string, string> param)
        {
            try
            {
                _reader.WriteItem("MBTPATH", param["MBTPATH"]);
                _reader.WriteItem("HEATTIME", param["HEATTIME"]);
                _reader.WriteItem("COM_ERROR_TOLARENCE", param["COM_ERROR_TOLARENCE"]);
                _reader.WriteItem("Condition_Timeout", param["Condition_Timeout"]);
                _reader.WriteItem("FloorCount", param["FloorCount"]);
                _reader.WriteItem("OVEN_PORT", param["OVEN_PORT"]);
                _reader.WriteItem("Floor1", param["Floor1"]);
                _reader.WriteItem("Floor2", param["Floor2"]);
                _reader.WriteItem("Floor3", param["Floor3"]);
                _reader.WriteItem("Floor4", param["Floor4"]);
                _reader.WriteItem("Floor5", param["Floor5"]);
                _reader.WriteItem("Floor6", param["Floor6"]);
                _reader.WriteItem("Floor7", param["Floor7"]);
                _reader.WriteItem("Floor8", param["Floor8"]);
                _reader.WriteItem("Floor9", param["Floor9"]);
                _reader.WriteItem("Floor10", param["Floor10"]);
                _reader.WriteItem("TbiesServer", param["TbiesServer"]);

                _reader.WriteItem("MES_SERVICE", param["MES_SERVICE"]);
               
                _reader.WriteItem("SWITCH", param["SWITCH"]);
                _reader.WriteItem("TIMEOUT", param["TIMEOUT"]);
                _reader.WriteItem("PORTNAME", param["PORTNAME"]);

            }
            catch (Exception ex)
            {
                _logger.Error("save config exception:" + ex.Message);
               
            }
        }
       
    }
}

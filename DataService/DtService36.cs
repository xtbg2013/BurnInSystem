using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Threading;
using log4net;
namespace BILib
{
    internal class ServiceTable
    {
        public string ServiceConnectStr { set; get; }
        public string LocalTable { set; get; }
        public string RemoteTable { set; get; }
    }
    internal class DtService36 :IDataService
    {
        private readonly DbService _dbService;
        private static  Thread _backDataService;
        private static bool _isBackServiceRun;
        private System.Timers.Timer _timer;
        private readonly List<ServiceTable> _centralServiceTable;
        private readonly ILog _log;
        public DtService36(string constr)
        {
            _log = LogManager.GetLogger("DataServiceLog");
            _dbService = new DbService(_log,constr);
            _centralServiceTable = new List<ServiceTable>();
            InitTimer();
        }
        public DtService36(string dataSource,string dataBaseName, string userId, string pwd)
        {
            _log = LogManager.GetLogger("DataServiceLog");
            _centralServiceTable = new List<ServiceTable>();
            var sqlConnectedStr = GetSqlConnectStr(dataSource, dataBaseName, userId, pwd);
            _dbService = new DbService(_log,sqlConnectedStr);
            InitTimer();
        }
        public bool IsTableExist(string conStr, string tableName)
        {
            var  log = LogManager.GetLogger("DataServiceLog");
            var service = new DbService(log,conStr);
            var cmd = $"select top 1 * from {tableName}";
            return service.Execute(cmd);
        }

        public string GetSqlConnectStr(string dataSource, string dataBaseName, string userId, string pwd)
        {
            const string constr = @"Data Source = {0}; Initial Catalog = {1}; Persist Security Info = True; User ID = {2}; Password = {3}; Pooling = False";
            object[] param = { dataSource, dataBaseName, userId, pwd };
            return string.Format(constr, param);
        }

        public bool CreateDataBase(string dataSource, string userId, string pwd, string sqlScriptPath,out string msg)//string server,string user,string pwd,string sqlScriptPath
        {
            var res = true;
            msg = "";
            const string constr = @"-S {0} -U {1} -P {2}  -i {3}";
            object[] param = { dataSource, userId, pwd, sqlScriptPath};
            try
            {
                var sqlProcess = new System.Diagnostics.Process
                {
                    StartInfo =
                    {
                        FileName = "osql.exe ",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        Arguments = string.Format(constr, param)
                    }
                };
                sqlProcess.Start();
                sqlProcess.WaitForExit();
                System.IO.StreamReader sr = sqlProcess.StandardOutput;
                msg = sr.ReadToEnd();
                _log.Info(msg);
                sqlProcess.Close();
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                _log.Info($"Run sql script error,message = {msg}");
                res = false;
            }
            return res;
        }
        public Guid ApplyDataSetId(string sn)
        {
            var id = Guid.NewGuid();
            var cmd = $"INSERT INTO BI_Unit (ID,SN,Create_Time) VALUES ('{id}','{sn}',GETDATE())";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert unit data into BI_Unit error,cmd={cmd}");
            }

            cmd = $"INSERT INTO BI_Unit_Temp (Data_Set_ID,SN,Create_Time,Uploaded) VALUES ('{id}','{sn}',GETDATE(),1)";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert unit data into BI_Unit_Temp error,cmd={cmd}");
            }
            return id;
        }

        public void InsertData(Guid dataSetId, string tag, List<KeyValuePair<string, string>> data)
        {
            var dict = new Dictionary<string,string>();
            foreach(var item in data)
                dict[item.Key]=item.Value;
            var value = new JavaScriptSerializer().Serialize(dict);
            var cmd = $"INSERT INTO BI_Data (Data_Set_ID,Tag,Value,Load_Time) VALUES ('{dataSetId}','{tag}','{value}',GETDATE())";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert bi data into BI_Data error,cmd={cmd}");
            }

            InsertTempData(dataSetId,tag,value); 
        }

        public string GetUnitLastResult(string sn)
        {
            var cmd =$"SELECT [Value] FROM BI_Data WHERE Tag = 'END' AND [Data_Set_ID] = (SELECT TOP 1 ID FROM BI_Unit WHERE[SN] = '{sn}' ORDER BY[Create_Time] DESC)";
            var result = _dbService.Query(cmd);
            if (result.Rows.Count == 0)
                return "UNKNOW";
            var data = new JavaScriptSerializer().Deserialize<Dictionary<string,string>>((string)result.Rows[0]["Value"]);
            return data["Result"];
        }

        public DataTable FetchData(string[] snSet, string[] paraSet, string startTime, string endTime,string tag)
        {
            var ret = new DataTable();
            ret.Columns.AddRange(new[] { new DataColumn("ID"), new DataColumn("SN"), new DataColumn("Load_Time") });
            ret.Columns.AddRange((from x in paraSet select new DataColumn(x)).ToArray());
            var snSetString = snSet.Aggregate("", (current, sn) => current + (current == "" ? "" : @",") + $"\'{sn}\'");
            var cmd = $@"SELECT * FROM BI_Unit as U,BI_Data as D WHERE U.[SN] in ({snSetString}) AND U.[ID]=D.[Data_Set_ID] 
                            AND Load_Time>='{startTime}' AND Load_Time<='{endTime}'
                            AND [Tag]='{tag}'
                            ORDER BY U.[SN],D.[Load_Time] ASC";
            var result = _dbService.Query(cmd);
            var serializer = new JavaScriptSerializer();
            foreach (DataRow row in result.Rows)
            {
                var dict = serializer.Deserialize<Dictionary<string, string>>((string)row["Value"]);
                if (paraSet.Length == 0)
                    foreach (var item in dict)
                        if (ret.Columns.Contains(item.Key) == false)
                            ret.Columns.Add(new DataColumn(item.Key));
                var entry = ret.NewRow();
                entry["ID"] = row["ID"];
                entry["SN"] = row["SN"];
                entry["Load_Time"] = row["Load_Time"];
                foreach (var item in dict)
                    entry[item.Key] = item.Value;
                ret.Rows.Add(entry);
            }
            return ret;
        }
        
        public string[] GetUnitParaList(Guid id)
        {
            var cmd = $@"SELECT TOP 1 * FROM BI_DATA WHERE [Data_Set_ID]='{id}' AND [Tag]='DATA'";
            var result = _dbService.Query(cmd);
            if (result.Rows.Count == 0)
                return new string[0];
            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string,string>>((string) result.Rows[0]["Value"]);
            return dict.Keys.ToArray();
        }

        public DataTable FetchUnitData(Guid id, int range = 200)
        {
            var ret = new DataTable();
            ret.Columns.Add(new DataColumn("Load_Time"));
            var cmd =$@"SELECT * FROM (SELECT TOP {range} * FROM BI_Data WHERE Data_Set_ID='{id}' AND Tag='DATA' ORDER BY Load_Time DESC) AS t ORDER BY Load_Time ASC";
                   
            var result = _dbService.Query(cmd);
            var serializer = new JavaScriptSerializer();
            foreach (DataRow row in result.Rows)
            {
                var dict = serializer.Deserialize<Dictionary<string,string>>((string)row["Value"]);
                foreach (var item in dict)
                    if (ret.Columns.Contains(item.Key) == false)
                        ret.Columns.Add(new DataColumn(item.Key));
                var entry = ret.NewRow();
                entry["Load_Time"] = row["Load_Time"];
                foreach (var item in dict)
                    entry[item.Key] = item.Value;
                ret.Rows.Add(entry);
            }
            return ret;
        }

        public DataTable GetValidSpecificationTable()
        {
            var cmd = "SELECT * FROM [BI_Specification] WHERE Validation=1";
            return _dbService.Query(cmd);
        }



        public string[] GetMapSchemsName()
        {
            const string cmd = "SELECT ID,SchemeName FROM [BI_Map]";
            var result = _dbService.Query(cmd);
            if (result.Rows.Count == 0)
                return new string[0];
            var dict = new Dictionary<string, string>();
            foreach (DataRow row in result.Rows)
            {
                dict[row[0].ToString()] = row[1].ToString();    
            }
            return dict.Values.ToArray();
        }
        public DataTable GetMapScheme(string mapSchemeName)
        {
            
            var cmd = $"SELECT * FROM [BI_Map] WHERE SchemeName ='{mapSchemeName}'";
            var result = _dbService.Query(cmd); 
            return result.Rows.Count == 0 ? new DataTable() : result;
        }
        public DataTable GetMapScheme(bool validation)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"SELECT * FROM [BI_Map] WHERE Validation ={temp}";
            var result = _dbService.Query(cmd);
            return result.Rows.Count == 0 ? new DataTable() : result;
        }

        public void DeleteMapScheme(string mapSchemeName)
        {
            var cmd = $"DELETE  FROM [BI_Map] WHERE SchemeName ='{mapSchemeName}'";
            _dbService.Query(cmd);
        }
        
        public void InsertMapScheme(string mapSchemeName, string content, int boardRows, int boardCols, int seatRows, int seatCols, bool validation = false)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"INSERT INTO BI_Map (SchemeName,MapContent,BoardRow,boardCol,SeatRow,SeatCol,Validation) VALUES ('{mapSchemeName}','{content}',{boardRows},{boardCols},{seatRows},{seatCols},{temp})";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert INTO BI_Map error,cmd={cmd}");
            }
        }
        public void UpdateMapScheme(string mapSchemeName, string content)
        {
            var cmd =$"Update BI_Map SET MapContent='{content}' WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }

        public void UpdateMapScheme(string mapSchemeName, string content, int boardRows, int boardCols , int seatRows, int seatCols)
        {
            var cmd = $"Update BI_Map SET MapContent='{content}',BoardRow ={boardRows},BoardCol ={boardCols},SeatRow ={seatRows},SeatCol ={seatCols} WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }
        
        public void UpdateMapScheme(string mapSchemeName, string content, bool validation)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"Update BI_Map SET MapContent='{content}',Validation={temp} WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }

        
        public void RigisterBakupDataTable(string centralConnectStr, string localTable, string remoteTable)
        {
            _log.Info($"Rigister central service:sql constr:{centralConnectStr},local table:{localTable},remote table:{remoteTable}");
            var table = new ServiceTable
            {
                ServiceConnectStr = centralConnectStr,
                LocalTable = localTable,
                RemoteTable = remoteTable
            };
            _centralServiceTable.Add(table);
        }
        public bool StartBakService(bool enable)
        {
            if (_isBackServiceRun && enable)
            {
                _log.Info("Bak service has been started");
                return false;
            }

            if (!_isBackServiceRun && !enable)
            {
                _log.Info("Bak service has been stopped");
                return false;
                
            }
            if (enable)
            {
                _log.Info("Start bak service");
                _backDataService = new Thread(BackupService);
                _backDataService.Start();
                _timer.Enabled = true;
            }
            else
            {
                _log.Info("Stop bak service");
                _isBackServiceRun = false;
                _timer.Enabled = false;
                _backDataService.Abort();

            }
            return true;
        }

        public bool GetBakServiceStatus()
        {
            return _isBackServiceRun;
        }

        public void BakupData()
        {
            foreach (var serviceTable in _centralServiceTable)
            {
                var table = FetchLocalUploadData(serviceTable.LocalTable);
                if(table.Rows.Count<=0)continue;
                var tableTemp = table.Copy();
                tableTemp.Columns.Remove("ID");
                
                BakupData(serviceTable.ServiceConnectStr, tableTemp, serviceTable.RemoteTable);
                UpdateLocalTempTableUpload(serviceTable.LocalTable, table, false);
                _log.Info($"Bak data:[local table:{serviceTable.LocalTable},remote table:{serviceTable.RemoteTable}]");
            }
        }
        public void DeleteLocalHaveUploadedTempData(string localTable)
        {
            _log.Info($"Delete local tempory datas which have been uploaded: local table:{localTable}");
            var cmd = $"DELETE {localTable} where [Uploaded]= 0";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"DELETE {localTable} error,cmd={cmd}");
            }
        }


       
        private static void BakupData(string connectionString,DataTable table,string remoteTable)
        {
            
            using (var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.KeepIdentity))
            {
                foreach (DataColumn col in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = remoteTable;
                bulkCopy.WriteToServer(table);
            }
           
        }
        private DataTable FetchLocalUploadData(string localTable)
        {
            var cmd = $"SELECT TOP 1000 * from {localTable} WHERE Uploaded = 1";
            var result = _dbService.Query(cmd);
            return result;
        }

        private void InsertTempData(Guid dataSetId,string tag,string value)
        {
            switch (tag)
            {
                case "END":
                    var table = FetchSummaryBiData(dataSetId.ToString());
                    SummaryLocalBiData(table);
                    break;
                case "DATA":
                    var serializer = new JavaScriptSerializer();
                    var dict = serializer.Deserialize<Dictionary<string, string>>(value);
                    var station="";
                    if (dict.ContainsKey("PCName"))
                        station = dict["PCName"];
                    var cmd = $"INSERT INTO BI_Data_Temp(Data_Set_ID,Station,Tag,Value,Load_Time,Uploaded) VALUES ('{dataSetId.ToString()}','{station}','{tag}','{value}',GETDATE(),{1})";
                    if (!_dbService.Execute(cmd))
                    {
                        _log.Error($"INSERT BI_Data_Temp error,cmd={cmd}");
                    }
                    break;
            }

        }

        private void UpdateLocalTempTableUpload(string localTable, DataTable table, bool status)
        {
            var upload = status ? 1 : 0;
            foreach (DataRow row in table.Rows)
            {
                var cmd = $"Update {localTable} set [Uploaded]={upload} where id= {row["ID"]}";
                if (!_dbService.Execute(cmd))
                {
                    _log.Error($"Update {localTable} error,cmd={cmd}");
                }
                 
            }

        }
        private void BackupService()
        {
            _isBackServiceRun = true;
            while (_isBackServiceRun)
            {
                try
                {
                    BakupData();
                }
                catch (Exception ex)
                {
                    _log.Error($"BackData error,message = {ex.Message}");
                }
                
                Thread.Sleep(20);
            }
        }
        
        private void InitTimer()
        {
            const int timeSpan = 1000*60*60;
            _timer = new System.Timers.Timer(timeSpan);
            _timer.Elapsed += DeleteLocalTempData;
            _timer.AutoReset = true;
        }
        private void DeleteLocalTempData(object source, System.Timers.ElapsedEventArgs e)
        {
            foreach (var serviceTable in _centralServiceTable)
            {
                DeleteLocalHaveUploadedTempData(serviceTable.LocalTable);
                
            }
        }
       
        private void SummaryLocalBiData(DataTable table)
        {

            for (var i = 0; i < table.Rows.Count; i++)
            {
                var station = table.Rows[i]["Station"].ToString();
                var sn = table.Rows[i]["SN"].ToString();
                var plan = table.Rows[i]["Plan"].ToString();
                var createTime = table.Rows[i]["Create_Time"].ToString();
                var finishTime = table.Rows[i]["Load_Time"].ToString();
                var board = table.Rows[i]["Board"].ToString();
                var slot = table.Rows[i]["Seat"].ToString();
                var costTime = table.Rows[i]["Cost"].ToString();
                var result = table.Rows[i]["Result"].ToString();
                var comment = table.Rows[i]["Comment"].ToString();
                var datasetid = table.Rows[i]["Data_Set_ID"].ToString();
                const string uploaded = "1";


                var cmd = $@"INSERT INTO [BMS36].[dbo].[BI_Data_Summary] 
                                (
                                [Station],
                                [SN],
                                [Plan],
                                [Create_Time],
                                [Finish_Time],
                                [Board],
                                [Slot],
                                [Cost_Time],
                                [Result],
                                [Comment],
                                [Uploaded],
                                [Load_Time]
                                )
                            VALUES 
                                (
                                '{station}',
                                '{sn}',
                                '{plan}',
                                cast('{createTime}' as datetime),
                                cast('{finishTime}' as datetime),
                                '{board}',
                                '{slot}',
                                '{costTime}',
                                '{result}',
                                '{comment}',
                                {uploaded},
                                    GETDATE()
                                )";

                 
                if (!_dbService.Execute(cmd))
                {
                    _log.Error($"INSERT INTO [BMS36].[dbo].[BI_Data_Summary] error,cmd={cmd}");
                }
                UpdateLocalBiUnitUpload(datasetid);
            }


        }
        private void UpdateLocalBiUnitUpload(string datasetid)
        {
            var cmd = $"Update [dbo].[BI_Unit] set [Uploaded]=1 where id= '{datasetid}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update [dbo].[BI_Unit] error,cmd={cmd}");
            }
        }
        private DataTable FetchSummaryBiData(string dataSetId)
        {
            var ret = new DataTable();
            ret.Columns.AddRange(new[] { new DataColumn("Create_Time"), new DataColumn("Load_Time"), new DataColumn("Data_Set_ID") });
           
            var cmd = $@"SELECT sta.Data_Set_ID Data_Set_ID, 
                                sta.Value ValueStart, 
                                sen.Value ValueEnd,
                                sta.Load_Time Create_Time, 
                                sen.Load_Time Load_Time
                         FROM   [dbo].BI_Data as sta,[dbo].BI_Data as sen
                         WHERE  sta.Data_Set_ID=sen.Data_Set_ID 
                                and sta.Tag='START' 
                                and sen.Tag='END' 
                                and sen.Data_Set_ID
                                IN 
                                  ( SELECT id 
                                    FROM [dbo].BI_Unit 
                                    WHERE id 
                                        IN 
                                        (
                                            SELECT distinct Data_Set_ID from  [dbo].BI_Data sen
                                            WHERE Data_Set_ID= '{dataSetId}' and [Uploaded]=0 
                                          )
                                   )";

            var result = _dbService.Query(cmd);
            var serializer = new JavaScriptSerializer();
            foreach (DataRow row in result.Rows)
            {
                var dictStart = serializer.Deserialize<Dictionary<string, string>>((string)row["ValueStart"]);
                var dictEnd = serializer.Deserialize<Dictionary<string, string>>((string)row["ValueEnd"]);
               
                foreach (var item in dictStart)
                    if (ret.Columns.Contains(item.Key) == false)
                        ret.Columns.Add(new DataColumn(item.Key));
                foreach (var item in dictEnd)
                    if (ret.Columns.Contains(item.Key) == false)
                        ret.Columns.Add(new DataColumn(item.Key));
              
                var entry = ret.NewRow();
                foreach (var item in dictStart)
                    entry[item.Key] = item.Value;
                foreach (var item in dictEnd)
                    entry[item.Key] = item.Value;
                entry["Create_Time"] = row["Create_Time"];
                entry["Load_Time"] = row["Load_Time"];
                entry["Data_Set_ID"] = row["Data_Set_ID"];
                ret.Rows.Add(entry);

            }
            return ret;
        }
 
    }
}

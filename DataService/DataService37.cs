using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using log4net;

namespace BILib
{
    internal class BakupTable
    {
        public string RemoteConnectStr { set; get; }
        public string LocalBiDataTable { set; get; }
        public string RemoteBiDataTable { set; get; }
        public string LocalBiSummaryDataTable { set; get; }
        public string RemoteBiSummaryDataTable { set; get; }

        public string LocalBiSpecificationTable { set; get; }
        public string RemoteBiBiSpecificationTable { set; get; }
        public string LocalBiMap { set; get; }
    }
    enum BurnState
    {
        Created = 1,
        Starting = 2,
        End = 3,
        Delete = 4
    }
    internal class DataService37:IDatabaseService
    {
        private readonly DatabaseOperate _dbService;
        private static Thread _backDataService;
        private static bool _isBackServiceRun;
        private System.Timers.Timer _timer;
        private readonly ILog _log;
        private int _deletePeriodDays;
        private int _bakupPeriodMinutes;
        public DataService37(string constr)
        {
            _deletePeriodDays = 1;
            _bakupPeriodMinutes = 1;
            _log = LogManager.GetLogger("DataServiceLog");
            _dbService = new DatabaseOperate(_log, constr);
            DataTables = new BakupTable
            {
                LocalBiDataTable = "BI_Data",
                RemoteBiDataTable = "BI_Data_TEST",
                LocalBiSummaryDataTable = "BI_Data_Summary",
                RemoteBiSummaryDataTable = "BI_Data_Summary_TEST",
                LocalBiSpecificationTable = "BI_Specification",
                RemoteBiBiSpecificationTable = "BI_Specification",
                LocalBiMap = "BI_Map"
            };
            InitTimer();
        }

        public BakupTable DataTables { get; }

        public bool IsTableExist(string conStr, string tableName)
        {
            var service = new DatabaseOperate(_log, conStr);
            var cmd = $"select top 1 * from {tableName}";
            
            return service.Execute(cmd);
        }
        public bool CreateDataBase(string dataSource, string userId, string pwd, string sqlScriptPath, out string msg)//string server,string user,string pwd,string sqlScriptPath
        {
            var res = true;
            msg = "";
            const string constr = @"-S {0} -U {1} -P {2}  -i {3}";
            object[] param = { dataSource, userId, pwd, sqlScriptPath };
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

        public Guid CreateBiRecord(string sn, string productType, string testPlan, string board, string slot)
        {
            var id = Guid.NewGuid();
            var cmd = $"insert into  {DataTables.LocalBiSummaryDataTable} (DataSetId,Sn,ProductType,Station,TestPlan,Board,Slot,Flag) VALUES " +
                      $"('{id}','{sn}','{productType}','{Environment.MachineName}','{testPlan}','{board}','{slot}',{(int)BurnState.Created} )";
            if (!_dbService.Execute(cmd))
            {
                 throw new Exception($"Insert bi record into BI_Data_Summary error,cmd={cmd}");
            }
            return id;
        }

        public bool UpdateBiReocordData(Guid dataId, DateTime startTime)
        {
            var cmd = $"update {DataTables.LocalBiSummaryDataTable} set StartTime='{startTime}' where DataSetId = '{dataId}'";
            if (_dbService.Execute(cmd)) return true;
            _log.Error($"Update BI_Data_Summary error,cmd={cmd}");
            return false;
        }
        public bool UpdateBiReocordData(Guid dataId, DateTime finishTime, string costTime, string result,string comment="")
        {
            var cmd = $"update {DataTables.LocalBiSummaryDataTable} set FinishTime='{finishTime}',CostTime='{costTime}',Result='{result}',Comment='{comment}',Flag={(int)BurnState.End}" +
                      $" where DataSetId = '{dataId}'";
            if (_dbService.Execute(cmd)) return true;
            _log.Error($"Update BI_Data_Summary error,cmd={cmd}"); 
            return false;
        }

        public bool InsertBiData(Guid dataId, List<KeyValuePair<string, string>> data)
        {
            var dict = new Dictionary<string, string>();
            foreach (var item in data)
                dict[item.Key] = item.Value;
            var value = new JavaScriptSerializer().Serialize(dict);
            var cmd = $"INSERT INTO {DataTables.LocalBiDataTable} (DataSetID,Value,MonitorTime,Flag) VALUES ('{dataId}','{value}',GETDATE(),{(int)BurnState.Created})";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert bi data into BI_Data error,cmd={cmd}");
                return false;
            }
            return true; 
        }

        public string[] GetBiDataItems(Guid id)
        {
            var cmd = $@"SELECT TOP 1 * FROM {DataTables.LocalBiDataTable} WHERE [DataSetId]='{id}'";
            var result = _dbService.Query(cmd);
            if (result.Rows.Count == 0)
                return new string[0];
            var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>((string)result.Rows[0]["Value"]);
            return dict.Keys.ToArray();
        }
        public DataTable GetBiData(Guid id, int range = 200)
        {
            var ret = new DataTable();
            ret.Columns.AddRange(new[] { new DataColumn("DataSetId")});
            var cmd = $@"SELECT TOP {range} * FROM {DataTables.LocalBiDataTable} WHERE [DataSetId]='{id}'";
            var result = _dbService.Query(cmd);
            var serializer = new JavaScriptSerializer();
            foreach (DataRow row in result.Rows)
            {
                var dict = serializer.Deserialize<Dictionary<string, string>>((string)row["Value"]);
                foreach (var item in dict)
                    if (ret.Columns.Contains(item.Key) == false)
                        ret.Columns.Add(new DataColumn(item.Key));
                var entry = ret.NewRow();
                entry["DataSetId"] = id;
                 
                foreach (var item in dict)
                    entry[item.Key] = item.Value;
                ret.Rows.Add(entry);
            }
            return ret;
        }
        public DataTable GetBiRecord(string[] snSet, string result = "")
        {
            var snSetString = snSet.Aggregate("", (current, sn) => current + (current == "" ? "" : @",") + $"\'{sn}\'");
            var cmd = "";
            cmd = result == "" ? $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Sn in ({snSetString}) ORDER BY CreateTime ASC" : $"SELECT * FROM BI_Data_Summary WHERE Sn in ({snSetString}) AND Result='{result}' ORDER BY CreateTime ASC";
            var ret = _dbService.Query(cmd);
            return ret;
        }

        public DataTable GetBiRecord(DateTime createTime, DateTime finishTime, string result = "")
        {
            var cmd = "";
            if (result == "")
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE CreateTime>='{createTime}' AND FinishTime <='{finishTime}' ORDER BY CreateTime ASC";
            }
            else
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE CreateTime>='{createTime}' AND FinishTime <='{finishTime}' AND Result='{result}' ORDER BY CreateTime ASC";
            }
            var ret = _dbService.Query(cmd);
            return ret;
        }

        public DataTable GetBiRecord(string[] snSet, DateTime createTime, DateTime finishTime, string result = "")
        {
            var snSetString = snSet.Aggregate("", (current, sn) => current + (current == "" ? "" : @",") + $"\'{sn}\'");
            var cmd = "";
            if (result == "")
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Sn in ({snSetString}) AND CreateTime>='{createTime}' AND FinishTime <='{finishTime}' ORDER BY CreateTime ASC";
            }
            else
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Sn in ({snSetString}) AND CreateTime>='{createTime}' AND FinishTime <='{finishTime}' AND Result='{result}' ORDER BY CreateTime ASC";
            }
            var ret = _dbService.Query(cmd);
            return ret;
        }
        public DataTable GetBiRecord(string station, string[] snSet, string result = "")
        {
            var cmd = "";
            if (result == "")
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Station='{station}' ORDER BY CreateTime ASC";
            }
            else
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Station='{station}' AND Result='{result}' ORDER BY CreateTime ASC";
            }
            var ret = _dbService.Query(cmd);
            return ret;
        }

        public DataTable GetBiRecord(string station, DateTime createTime, DateTime finishTime, string result = "")
        {
            var cmd = "";
            if (result == "")
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Station='{station}' AND CreateTime>='{createTime}' AND FinishTime <='{finishTime}' ORDER BY CreateTime ASC";
            }
            else
            {
                cmd = $"SELECT * FROM {DataTables.LocalBiSummaryDataTable} WHERE Station='{station}' AND CreateTime>='{createTime}' AND FinishTime <='{finishTime}' AND Result='{result}' ORDER BY CreateTime ASC";
            }
            var ret = _dbService.Query(cmd);
            return ret;
        }

        

        public DataTable GetValidSpecificationTable()
        {
            var cmd = $"SELECT * FROM {DataTables.LocalBiSpecificationTable} WHERE Validation=1";
            return _dbService.Query(cmd);
        }
        public string[] GetMapSchemsName()
        {
            var cmd = $"SELECT ID,SchemeName FROM {DataTables.LocalBiMap}";
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

            var cmd = $"SELECT * FROM {DataTables.LocalBiMap} WHERE SchemeName ='{mapSchemeName}'";
            var result = _dbService.Query(cmd);
            return result.Rows.Count == 0 ? new DataTable() : result;
        }
        public DataTable GetMapScheme(bool validation)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"SELECT * FROM {DataTables.LocalBiMap} WHERE Validation ={temp}";
            var result = _dbService.Query(cmd);
            return result.Rows.Count == 0 ? new DataTable() : result;
        }
        public void DeleteMapScheme(string mapSchemeName)
        {
            var cmd = $"DELETE  FROM {DataTables.LocalBiMap} WHERE SchemeName ='{mapSchemeName}'";
            _dbService.Query(cmd);
        }

        public void InsertMapScheme(string mapSchemeName, string content, int boardRows, int boardCols, int seatRows, int seatCols, bool validation = false)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"INSERT INTO {DataTables.LocalBiMap} (SchemeName,MapContent,BoardRow,boardCol,SeatRow,SeatCol,Validation) VALUES ('{mapSchemeName}','{content}',{boardRows},{boardCols},{seatRows},{seatCols},{temp})";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Insert INTO BI_Map error,cmd={cmd}");
            }
        }
        public void UpdateMapScheme(string mapSchemeName, string content)
        {
            var cmd = $"Update {DataTables.LocalBiMap} SET MapContent='{content}' WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }

        public void UpdateMapScheme(string mapSchemeName, string content, int boardRows, int boardCols, int seatRows, int seatCols)
        {
            var cmd = $"Update {DataTables.LocalBiMap} SET MapContent='{content}',BoardRow ={boardRows},BoardCol ={boardCols},SeatRow ={seatRows},SeatCol ={seatCols} WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }

        public void UpdateMapScheme(string mapSchemeName, string content, bool validation)
        {
            var temp = validation ? 1 : 0;
            var cmd = $"Update {DataTables.LocalBiMap} SET MapContent='{content}',Validation={temp} WHERE SchemeName = '{mapSchemeName}'";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"Update BI_Map error,cmd={cmd}");
            }
        }

        
        //remote data service
        public void SetDeletePeriod(int days)
        {
            _deletePeriodDays = days;
        }

        public void SetBakupPeriod(int minutes)
        {
            _bakupPeriodMinutes = minutes;
        }

        private void InitTimer()
        {
            var timeSpan = _deletePeriodDays * 24 * 60 * 60*1000; 
            _timer = new System.Timers.Timer(timeSpan);
            _timer.Elapsed += DeleteLocalData;
            _timer.AutoReset = true;
        }
        public void SetRemoteConStr(string remoteConStr)
        {
            DataTables.RemoteConnectStr = remoteConStr;
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
        public void TransDataToServer()
        {
            var table = BakupData(DataTables.RemoteConnectStr,DataTables.LocalBiSummaryDataTable,DataTables.RemoteBiSummaryDataTable);
            UpdateTableUploadStatus(DataTables.LocalBiSummaryDataTable, table, BurnState.Starting);

            table = BakupData(DataTables.RemoteConnectStr, DataTables.LocalBiDataTable, DataTables.RemoteBiDataTable);
            UpdateTableUploadStatus(DataTables.LocalBiDataTable, table, BurnState.Delete);
            UpdateRemoteBiSummaryData(DataTables.RemoteConnectStr);
        }

        private DataTable BakupData(string remoteConStr,string localTable,string remoteTable)
        {
            var table = FetchLocalUploadData(localTable, BurnState.Created);
            if (table.Rows.Count > 0)
            {
                var tableData = table.Copy();
                tableData.Columns.Remove("Id");

                using (var bulkCopy = new SqlBulkCopy(remoteConStr, SqlBulkCopyOptions.KeepIdentity))
                {
                    foreach (DataColumn col in tableData.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = remoteTable;
                    bulkCopy.WriteToServer(tableData);
                }
                _log.Info($"Bak data:[local table:{localTable},remote table:{remoteTable}]");
            }

            return table;   
        }

        private void UpdateRemoteBiSummaryData(string remoteConStr)
        {
            var table = FetchLocalUploadData(DataTables.LocalBiSummaryDataTable, BurnState.End);
            if (table.Rows.Count <= 0)
                return ;
            //UPDATE A  SET A1 = B1, A2 = B2, A3 = B3  FROM A LEFT JOIN B ON A.ID = B.ID
            var remoteService = new DatabaseOperate(_log, remoteConStr);

            //var cmd = $"update R set " +
            //          $"R.Result= L.Result," +
            //          $"R.Comment = L.Comment,"+
            //          $"R.FinishTime = L.FinishTime," +
            //          $"R.CostTime = L.CostTime," +
            //          $"R.Flag = L.Flag "+
            //          $"from {DataTables.RemoteBiSummaryDataTable} R left join {DataTables.LocalBiSummaryDataTable} L ON R.DataSetId = L.DataSetId";

            foreach (DataRow row in table.Rows)
            {
                var cmd = $"update R set " +
                          $"R.Result= '{row["Result"]}'," +
                          $"R.Comment = '{row["Comment"]}'," +
                          $"R.FinishTime = '{row["FinishTime"]}'," +
                          $"R.CostTime = '{row["CostTime"]}'," +
                          $"R.Flag = {row["Flag"]} " +
                          $"from {DataTables.RemoteBiSummaryDataTable} R where R.DataSetId = '{row["DataSetId"]}'";
                if (!remoteService.Execute(cmd))
                {
                    _log.Error($"Update remote bi summary data  error,cmd={cmd}");

                }
                else
                {
                    UpdateTableUploadStatus(DataTables.LocalBiSummaryDataTable, table, BurnState.Delete);

                }

            }

            

            
            
        }


        private void BackupService()
        {
            _isBackServiceRun = true;
            var seconds = _bakupPeriodMinutes * 60;
            while (_isBackServiceRun)
            {
                try
                {
                    TransDataToServer();
                }
                catch (Exception ex)
                {
                    _log.Error($"BackData error,message = {ex.Message}");
                }

                Thread.Sleep(seconds);
            }
        }

      
        private void UpdateTableUploadStatus(string localTable, DataTable table, BurnState state)
        {
            
            foreach (DataRow row in table.Rows)
            {
                var cmd = $"Update {localTable} set [Flag]='{(int)state}' where id= {row["Id"]}";
                if (!_dbService.Execute(cmd))
                {
                    _log.Error($"Update {localTable} error,cmd={cmd}");
                }
            }

        }
       


        private void DeleteLocalData(object source, System.Timers.ElapsedEventArgs e)
        {
            DeleteHaveUploadedData(DataTables.LocalBiDataTable);
            DeleteHaveUploadedData(DataTables.LocalBiSummaryDataTable);
        }
        public void DeleteHaveUploadedData(string localTable)
        {
            _log.Info($"Delete local tempory datas which have been uploaded: local table:{localTable}");
            var cmd = $"DELETE {localTable} where [Flag]= {(int)BurnState.Delete}";
            if (!_dbService.Execute(cmd))
            {
                _log.Error($"DELETE {localTable} error,cmd={cmd}");
            }
        }

        public DataTable FetchLocalUploadData(string localTable, BurnState state)
        {
            var cmd = $"SELECT TOP 1000 * from {localTable} WHERE Flag = '{(int)state}'";
            var result = _dbService.Query(cmd);
            return result;
        }

        public bool DeleteTableData(string table)
        {
            var cmd = $"Delete {table}";
            return _dbService.Execute(cmd);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;

namespace BILib
{
    public interface IDatabaseService
    {
        bool CreateDataBase(string dataSource, string userId, string pwd, string sqlScriptPath, out string msg);
        bool IsTableExist(string conStr, string tableName);
        Guid CreateBiRecord(string sn,string productType,string testPlan,string board,string slot);
        bool UpdateBiReocordData(Guid dataId, DateTime startTime);
        bool UpdateBiReocordData(Guid dataId,DateTime finishTime, string costTime, string result,string comment="");
        bool InsertBiData(Guid dataId, List<KeyValuePair<string, string>> data);
        string[] GetBiDataItems(Guid id);
        DataTable GetBiData(Guid id, int range = 200);
        DataTable GetBiRecord(string[] snSet,string result = "");
        DataTable GetBiRecord(DateTime createTime, DateTime finishTime, string result = "");
        DataTable GetBiRecord(string[] snSet,DateTime createTime, DateTime finishTime, string result = "");
        DataTable GetBiRecord(string station, string[] snSet, string result = "");
        DataTable GetBiRecord(string station, DateTime createTime, DateTime finishTime, string result = "");
        
     
        DataTable GetValidSpecificationTable();
        string[] GetMapSchemsName();
        DataTable GetMapScheme(string mapSchemeName);
        DataTable GetMapScheme(bool validation);
        void DeleteMapScheme(string mapSchemeName);
        void InsertMapScheme(string mapSchemeName, string content, int boardRows, int boardCols, int seatRows, int seatCols, bool validation = false);
        void UpdateMapScheme(string mapSchemeName, string content);
        void UpdateMapScheme(string mapSchemeName, string content, int boardRows, int boardCols, int seatRows, int seatCols);
        void UpdateMapScheme(string mapSchemeName, string content, bool validation);

        //remote data service
        void SetDeletePeriod(int days);
        void SetBakupPeriod(int minutes);
        
        void SetRemoteConStr(string remoteConStr);
       
        bool StartBakService(bool enable);
        bool GetBakServiceStatus();
    }
}

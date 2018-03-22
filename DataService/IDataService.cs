using System;
using System.Collections.Generic;
using System.Data;

namespace BILib
{
    public interface IDataService
    {  
        string GetSqlConnectStr(string dataSource, string dataBaseName, string userId, string pwd);
        bool CreateDataBase(string dataSource,string userId, string pwd,string sqlScriptPath, out string msg);
        bool IsTableExist(string conStr,string tableName);
        Guid ApplyDataSetId(string sn);
        void InsertData(Guid dataSetId, string tag, List<KeyValuePair<string, string>> data);
        string GetUnitLastResult(string sn);
        DataTable FetchData(string[] snSet, string[] paraSet, string startTime, string endTime, string tag);
        string[] GetUnitParaList(Guid id);
        DataTable FetchUnitData(Guid id, int range=200);
        DataTable GetValidSpecificationTable();

        
        string[] GetMapSchemsName();
        DataTable GetMapScheme(string mapSchemeName);
        DataTable GetMapScheme(bool validation);
        void DeleteMapScheme(string mapSchemeName);
        void InsertMapScheme(string mapSchemeName, string content,int boardRows,int boardCols,int seatRows,int seatCols,bool validation = false);
        void UpdateMapScheme(string mapSchemeName, string content);

        void UpdateMapScheme(string mapSchemeName, string content,int boardRows, int boardCols, int seatRows,int seatCols);
        void UpdateMapScheme(string mapSchemeName, string content,bool validation);

        //remote data service
        void RigisterBakupDataTable(string centralConnectStr,string localTable, string remoteTable);
        bool StartBakService(bool enable);
        bool GetBakServiceStatus();

    }
}

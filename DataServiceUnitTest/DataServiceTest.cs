using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BILib;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Threading;

namespace DataServiceUnitTest
{
    [TestClass, Ignore]
    public class DataServiceTest
    {
        private string _dataSource = System.Environment.MachineName + @"\SQLEXPRESS";
        private string _dataBaseName = "BMS36";
        private string _userId = "sa";
        private string _pwd = "cml@shg629";

        private string GetConStr()
        {
            string _constr =
                @"Data Source={0}\SQLEXPRESS;Initial Catalog=BMS36;Persist Security Info=True;User ID=sa;Password=cml@shg629;Pooling=False";
            return string.Format(_constr, System.Environment.MachineName);
        }


        [TestMethod]
        public void Test_CreateDataService()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            Assert.AreNotEqual(iDbSrv, null);
        }


        [TestMethod]
        public void Test_CreateDatabase()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            string msg;
            bool res = iDbSrv.CreateDataBase(_dataSource, _userId, _pwd, "BMS361.sql", out msg);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_GetMapSchemeName()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);

            string[] res = iDbSrv.GetMapSchemsName();
        }

        [TestMethod]
        public void Test_GetMapScheme()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            iDbSrv.GetMapScheme("VBMS_POS_MAP");
        }


        [TestMethod]
        public void Test_InsertMapScheme()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            //iDbSrv.InsertMapScheme("2","{}");
            //iDbSrv.InsertMapScheme("3", "{}");
        }

        [TestMethod]
        public void Test_DeleteMapScheme()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            iDbSrv.DeleteMapScheme("2");
        }

        [TestMethod]
        public void Test_UpdateMapScheme()
        {
            IDataService iDbSrv = new DtService36(this._dataSource, this._dataBaseName, this._userId, this._pwd);
            iDbSrv.UpdateMapScheme("1", "{1}");
        }


        [TestMethod]
        public void TestInsertData()
        {
            var iDbSrv = DataServiceFactory.CreateDataService(_dataSource, _dataBaseName, _userId, _pwd);

            for (var i = 0; i < 2; i++)
            {
                var sn = $"TEST000{i}";
                var entry = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Station", Environment.MachineName),
                    new KeyValuePair<string, string>("SN", sn),
                    new KeyValuePair<string, string>("Plan", "A"),
                    new KeyValuePair<string, string>("Board", "A"),
                    new KeyValuePair<string, string>("Floor", "1"),
                    new KeyValuePair<string, string>("Seat", "1"),
                    new KeyValuePair<string, string>("Create_Time", "1"),
                    new KeyValuePair<string, string>("Load_Time", "1"),
                    new KeyValuePair<string, string>("Cost", "1"),
                    new KeyValuePair<string, string>("Result", "1"),
                    new KeyValuePair<string, string>("Comment", "1"),
                    new KeyValuePair<string, string>("Data_Set_ID", "1")
                };
                var id = iDbSrv.ApplyDataSetId(sn);
                iDbSrv.InsertData(id, "START", entry);
                iDbSrv.InsertData(id, "END", entry);
                iDbSrv.InsertData(id, "DATA", entry);
            }
        }

        [TestMethod]
        public void TestRigisterCentralService()
        {
            var iDbSrv = DataServiceFactory.CreateDataService(_dataSource, _dataBaseName, _userId, _pwd);
            iDbSrv.RigisterBakupDataTable(GetConStr(), "[BMS36].[dbo].[BI_Data_Summary]",
                "[BMS36].[dbo].[Remotee_BI_Data_Summary]");
        }

        [TestMethod]
        public void TestBakupData()
        {
            var iDbSrv = new DtService36(_dataSource, _dataBaseName, _userId, _pwd);
            iDbSrv.RigisterBakupDataTable(GetConStr(), "[BMS36].[dbo].[BI_Data_Summary]",
                "[BMS36].[dbo].[Remote_BI_Data_Summary]");
            iDbSrv.RigisterBakupDataTable(GetConStr(), "[BMS36].[dbo].[BI_Data_Temp]",
                "[BMS36].[dbo].[Remote_BI_Data]");
            for (var i = 0; i < 2; i++)
            {
                var sn = $"TEST000{i}";
                var entry = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Station", Environment.MachineName),
                    new KeyValuePair<string, string>("SN", sn),
                    new KeyValuePair<string, string>("Plan", "A"),
                    new KeyValuePair<string, string>("Board", "A"),
                    new KeyValuePair<string, string>("Floor", "1"),
                    new KeyValuePair<string, string>("Seat", "1"),
                    new KeyValuePair<string, string>("Create_Time", "1"),
                    new KeyValuePair<string, string>("Load_Time", "1"),
                    new KeyValuePair<string, string>("Cost", "1"),
                    new KeyValuePair<string, string>("Result", "1"),
                    new KeyValuePair<string, string>("Comment", "1"),
                    new KeyValuePair<string, string>("Data_Set_ID", "1")
                };
                var id = iDbSrv.ApplyDataSetId(sn);
                iDbSrv.InsertData(id, "START", entry);
                iDbSrv.InsertData(id, "END", entry);
                iDbSrv.InsertData(id, "DATA", entry);
            }

            iDbSrv.BakupData();
        }

        [TestMethod]
        public void TestDeleteHaveUploadedLocalTempData()
        {
            var iDbSrv = new DtService36(_dataSource, _dataBaseName, _userId, _pwd);
            iDbSrv.RigisterBakupDataTable(GetConStr(), "[BMS36].[dbo].[BI_Data_Summary]",
                "[BMS36].[dbo].[Remote_BI_Data_Summary]");
            for (var i = 0; i < 2; i++)
            {
                var sn = $"TEST000{i}";
                var entry = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Station", Environment.MachineName),
                    new KeyValuePair<string, string>("SN", sn),
                    new KeyValuePair<string, string>("Plan", "A"),
                    new KeyValuePair<string, string>("Board", "A"),
                    new KeyValuePair<string, string>("Floor", "1"),
                    new KeyValuePair<string, string>("Seat", "1"),
                    new KeyValuePair<string, string>("Create_Time", "1"),
                    new KeyValuePair<string, string>("Load_Time", "1"),
                    new KeyValuePair<string, string>("Cost", "1"),
                    new KeyValuePair<string, string>("Result", "1"),
                    new KeyValuePair<string, string>("Comment", "1"),
                    new KeyValuePair<string, string>("Data_Set_ID", "1")
                };
                var id = iDbSrv.ApplyDataSetId(sn);
                iDbSrv.InsertData(id, "START", entry);
                iDbSrv.InsertData(id, "END", entry);
            }

            iDbSrv.BakupData();
            iDbSrv.DeleteLocalHaveUploadedTempData("[BMS36].[dbo].[BI_Data_Summary]");
        }

        [TestMethod]
        public void TestStartBackService()
        {
            var iDbSrv = DataServiceFactory.CreateDataService(_dataSource, _dataBaseName, _userId, _pwd);
            var res = iDbSrv.StartBakService(true);
            Assert.AreEqual(res, true);
            res = iDbSrv.StartBakService(true);

            Assert.AreEqual(res, false);
            res = iDbSrv.StartBakService(false);
            Assert.AreEqual(res, true);
            res = iDbSrv.StartBakService(false);
            Assert.AreEqual(res, false);
        }

        [TestMethod]
        public void TestIsTableExist()
        {
            //var iDbSrv = DataServiceFactory.CreateDataService(_dataSource, _dataBaseName, _userId, _pwd);
            //var res = iDbSrv.IsTableExist("[dbo].[BI_Data_Temp]");
            //Assert.AreEqual(res, true);
            //res = iDbSrv.IsTableExist("[dbo].[BI_Data_Temp1]");
            //Assert.AreEqual(res, false);
        }
    }
}

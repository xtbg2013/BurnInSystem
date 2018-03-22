using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BILib;
using System.Collections.Generic;

namespace DataServiceUnitTest
{
    [TestClass]
    public class DataBaseServiceTest
    {
        public string DataSource { get; set; }
        public string DataBaseName { get; set; }
        public string UserId { get; set; }
        public string Pwd { get; set; }
        public string ConStr { get; set; }
        public IDatabaseService DataService { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            DataSource = Environment.MachineName + @"\SQLEXPRESS";
            DataBaseName = "BMS37";
            UserId = "sa";
            Pwd = "cml@shg629";
            ConStr = $"Data Source = {Environment.MachineName + @"\SQLEXPRESS"}; Initial Catalog = {DataBaseName}; Persist Security Info = True; User ID = {UserId}; Password = {Pwd}; Pooling = False";
            DataService = new DataService37(ConStr);
            Assert.AreNotEqual(DataService, null);
            var res = DataService.CreateDataBase(DataSource, UserId, Pwd, "BMS37.sql", out var _);
            Assert.AreEqual(res, true);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var service = new DataService37(ConStr);
            var res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiDataTable);
            Assert.AreEqual(res, true);
        }
        [TestMethod]
        public void Test_IsLocalTableExist()
        {
            var res = DataService.IsTableExist(ConStr,"BI_Data");
            Assert.AreEqual(res, true);

            res = DataService.IsTableExist(ConStr, "BI_Data_Summary");
            Assert.AreEqual(res, true);

            res = DataService.IsTableExist(ConStr, "BI_Map");
            Assert.AreEqual(res, true);

            res = DataService.IsTableExist(ConStr, "BI_Specification");
            Assert.AreEqual(res, true);

            res = DataService.IsTableExist(ConStr, "xxx");
            Assert.AreEqual(res, false);
        }

        [TestMethod,Ignore]
        public void Test_IsRemoteTableExist()
        {
            var connText = @"Data Source = wux-prod04\prod01; Initial Catalog = BI_Data; Persist Security Info = True; User ID = BI_Data_Admin; Password = BI_Data_Admin@123; Pooling = False";
            var service  = new DataService37(connText);


            var res = service.IsTableExist(connText, "BI_Data_Summary_TEST");
            Assert.AreEqual(res, true);

            res = service.IsTableExist(connText, "BI_Data_TEST");
            Assert.AreEqual(res, true);

            
        }

        [TestMethod]
        public void Test_CreateBiRecord()
        {
            DataService.CreateBiRecord("TEST_001", "QSFP28G","TEST_PLAN","L10_1","1");
            var service = new DataService37(ConStr);
            var table = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable,BurnState.Created);
            Assert.AreEqual(table.Rows.Count,1);
            var res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_UpdateBiReocordData1()
        {
            var id = DataService.CreateBiRecord("TEST_002", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            var res = DataService.UpdateBiReocordData(id,DateTime.Now);
            Assert.AreEqual(res, true);
            var service = new DataService37(ConStr);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }
        [TestMethod]
        public void Test_UpdateBiReocordData2()
        {
            var id = DataService.CreateBiRecord("TEST_003", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            DateTime finishTime = DateTime.Now;
            var res = DataService.UpdateBiReocordData(id, finishTime,"60","PASS");
            Assert.AreEqual(res, true);

            var service = new DataService37(ConStr);
            var table = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable, BurnState.End);
            Assert.AreEqual(table.Rows.Count, 1);
           
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_InsertBiData()
        {
            var id = DataService.CreateBiRecord("TEST_004", "QSFP28G", "TEST_PLAN", "L10_1", "1");
           
            var res = DataService.InsertBiData(id, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);

            var service = new DataService37(ConStr);
            var table = service.FetchLocalUploadData(service.DataTables.LocalBiDataTable, BurnState.Created);
            Assert.AreEqual(table.Rows.Count, 1);

            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_GetBiData()
        {
            var id = DataService.CreateBiRecord("TEST_005", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = DataService.InsertBiData(id, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            var table = DataService.GetBiData(id, 1);
            Assert.AreEqual(table.Rows.Count,1);

            table = DataService.GetBiData(id,2);
            Assert.AreEqual(table.Rows.Count, 2);

            table = DataService.GetBiData(id);
            Assert.AreEqual(table.Rows.Count, 2);

            var service = new DataService37(ConStr);
            
            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }
        [TestMethod]
        public void Test_GetBiRecord1()
        {
            var id1 = DataService.CreateBiRecord("TEST_006", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            DateTime finishTime = DateTime.Now;
            res = DataService.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);


            var id2 = DataService.CreateBiRecord("TEST_007", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            res = DataService.UpdateBiReocordData(id2, finishTime, "60", "FAIL");
            Assert.AreEqual(res, true);
            string[] snSet = new[] {"TEST_007", "TEST_006"};
            var table = DataService.GetBiRecord(snSet);
            Assert.AreEqual(table.Rows.Count,2);
            table = DataService.GetBiRecord(snSet, "PASS");
            Assert.AreEqual(table.Rows.Count,1);
            table = DataService.GetBiRecord(snSet, "FAIL");
            Assert.AreEqual(table.Rows.Count,1);

            var service = new DataService37(ConStr);

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_GetBiRecord2()
        {
            var id1 = DataService.CreateBiRecord("TEST_008", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            DateTime startIime = DateTime.Now.AddHours(-1);
            DateTime finishTime = DateTime.Now.AddHours(1);
             
            res = DataService.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);


            var id2 = DataService.CreateBiRecord("TEST_009", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            res = DataService.UpdateBiReocordData(id2, finishTime, "60", "FAIL");
            Assert.AreEqual(res, true);
             

            var table = DataService.GetBiRecord(startIime,finishTime);
            Assert.AreEqual(table.Rows.Count , 2);
            table = DataService.GetBiRecord(startIime, finishTime, "PASS");
            Assert.AreEqual(table.Rows.Count , 1);
            table = DataService.GetBiRecord(startIime, finishTime, "FAIL");
            Assert.AreEqual(table.Rows.Count , 1);
            var service = new DataService37(ConStr);

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_GetBiRecord3()
        {
            var id1 = DataService.CreateBiRecord("TEST_010", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            DateTime finishTime = DateTime.Now.AddHours(1);

            res = DataService.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);


            var id2 = DataService.CreateBiRecord("TEST_011", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            res = DataService.UpdateBiReocordData(id2, finishTime, "60", "FAIL");
            Assert.AreEqual(res, true);


            var table = DataService.GetBiRecord(Environment.MachineName, new[] { "TEST_010", "TEST_011" });
            Assert.AreEqual(table.Rows.Count , 2);
            table = DataService.GetBiRecord(Environment.MachineName, new[] { "TEST_010", "TEST_011" }, "PASS");
            Assert.AreEqual(table.Rows.Count , 1);
            table = DataService.GetBiRecord(Environment.MachineName, new[] { "TEST_010", "TEST_011" }, "FAIL");
            Assert.AreEqual(table.Rows.Count , 1);

            var service = new DataService37(ConStr);

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }


        [TestMethod]
        public void Test_GetBiRecord4()
        {
            var id1 = DataService.CreateBiRecord("TEST_012", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            var res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            DateTime startIime = DateTime.Now.AddHours(-1);
            DateTime finishTime = DateTime.Now.AddHours(1);

            res = DataService.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);


            var id2 = DataService.CreateBiRecord("TEST_013", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            res = DataService.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("c", "3"),
                new KeyValuePair<string, string>("d", "4")
            });
            Assert.AreEqual(res, true);
            res = DataService.UpdateBiReocordData(id2, finishTime, "60", "FAIL");
            Assert.AreEqual(res, true);


            var table = DataService.GetBiRecord(Environment.MachineName, startIime, finishTime);
            Assert.AreEqual(table.Rows.Count , 2);
            table = DataService.GetBiRecord(Environment.MachineName, startIime, finishTime, "PASS");
            Assert.AreEqual(table.Rows.Count , 1);
            table = DataService.GetBiRecord(Environment.MachineName, startIime, finishTime, "FAIL");
            Assert.AreEqual(table.Rows.Count , 1);

            var service = new DataService37(ConStr);

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void Test_TransDataToServer()
        {
            var service = new DataService37(ConStr);
            service.SetRemoteConStr(ConStr);
            var id1 = service.CreateBiRecord("ZWX_1", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            var res = service.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            service.TransDataToServer();
            var table  = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable,BurnState.Starting);
            Assert.AreEqual(table.Rows.Count,1);

            table = service.FetchLocalUploadData(service.DataTables.LocalBiDataTable, BurnState.Delete);
            Assert.AreEqual(table.Rows.Count, 1);
            table = service.FetchLocalUploadData(service.DataTables.RemoteBiSummaryDataTable, BurnState.Created);
            Assert.AreEqual(table.Rows.Count, 1);

            table = service.FetchLocalUploadData(service.DataTables.RemoteBiDataTable, BurnState.Created);
            Assert.AreEqual(table.Rows.Count, 1);
            

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiDataTable);



            Assert.AreEqual(res, true);

        }


        [TestMethod]
        public void Test_UpdateRemoteData()
        {
            var service = new DataService37(ConStr);

            service.SetRemoteConStr(ConStr);
            var id1 = service.CreateBiRecord("UPDATE_TEST1", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = service.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            service.TransDataToServer();
            DateTime finishTime = DateTime.Now;
            res = service.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);

            var table = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable, BurnState.End);
            Assert.AreEqual(table.Rows.Count, 1);

           

            service.TransDataToServer();

            table = service.FetchLocalUploadData(service.DataTables.RemoteBiSummaryDataTable, BurnState.End);
            Assert.AreEqual(table.Rows.Count, 1);

            table = service.FetchLocalUploadData(service.DataTables.RemoteBiDataTable, BurnState.Created);
            Assert.AreEqual(table.Rows.Count, 1);

            table = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable, BurnState.Delete);
            Assert.AreEqual(table.Rows.Count, 1);

            table = service.FetchLocalUploadData(service.DataTables.LocalBiDataTable, BurnState.Delete);
            Assert.AreEqual(table.Rows.Count, 1);

            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiDataTable);
            Assert.AreEqual(res, true);
        }
        [TestMethod]
        public void Test_DeleteHaveUploadedData()
        {

            var service = new DataService37(ConStr);
            service.SetRemoteConStr(ConStr);
            var id1 = service.CreateBiRecord("ZWX_1", "QSFP28G", "TEST_PLAN", "L10_1", "1");

            var res = service.InsertBiData(id1, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });
            Assert.AreEqual(res, true);
            
            var id2 = service.CreateBiRecord("ZWX_2", "QSFP28G", "TEST_PLAN", "L10_1", "1");
            res = service.InsertBiData(id2, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "1"),
                new KeyValuePair<string, string>("b", "2")
            });

            Assert.AreEqual(res, true);
            service.TransDataToServer();
            DateTime finishTime = DateTime.Now;
            res = service.UpdateBiReocordData(id1, finishTime, "60", "PASS");
            Assert.AreEqual(res, true);
            res = service.UpdateBiReocordData(id2, finishTime, "60", "FAIL");
            Assert.AreEqual(res, true);
            service.TransDataToServer();
            service.DeleteHaveUploadedData(service.DataTables.LocalBiSummaryDataTable);
            service.DeleteHaveUploadedData(service.DataTables.LocalBiDataTable);

            var table = service.FetchLocalUploadData(service.DataTables.LocalBiSummaryDataTable, BurnState.Delete);
            Assert.AreEqual(table.Rows.Count, 0);
            table = service.FetchLocalUploadData(service.DataTables.LocalBiDataTable, BurnState.Delete);
            Assert.AreEqual(table.Rows.Count, 0);
            res = service.DeleteTableData(service.DataTables.LocalBiDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.LocalBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiSummaryDataTable);
            Assert.AreEqual(res, true);
            res = service.DeleteTableData(service.DataTables.RemoteBiDataTable);
            Assert.AreEqual(res, true);
        }

        [TestMethod]
        public void TestStartBackService()
        {

            var res = DataService.StartBakService(true);
            Assert.AreEqual(res, true);
            res = DataService.StartBakService(true);

            Assert.AreEqual(res, false);
            res = DataService.StartBakService(false);
            Assert.AreEqual(res, true);
            res = DataService.StartBakService(false);
            Assert.AreEqual(res, false);
        }
        [TestMethod]
        public void Test_GetMapSchemeName()
        {
            var res = DataService.GetMapSchemsName();
        }

        [TestMethod]
        public void Test_GetMapScheme()
        {
            DataService.GetMapScheme("VBMS_POS_MAP");
        }


        [TestMethod]
        public void Test_InsertMapScheme()
        {

            //DataService.InsertMapScheme("2", "{}");
            //DataService.InsertMapScheme("3", "{}");
        }

        [TestMethod]
        public void Test_DeleteMapScheme()
        {

            DataService.DeleteMapScheme("2");
        }

        [TestMethod]
        public void Test_UpdateMapScheme()
        {

            DataService.UpdateMapScheme("1", "{1}");
        }


         
        [TestMethod]
        public void Test_RigisterCentralService()
        {
            DataService.SetRemoteConStr(ConStr);  
        }

       

        
    }
}

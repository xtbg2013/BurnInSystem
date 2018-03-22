using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using BIModel.Interface;
using BILib;
using BIModel;
using BIModel.Factory;
using BIModel.NowCoding;
using BIModel.Manager;
namespace BiModelUnitTest
{
    [TestClass]
    public class UnitTestBoardManager
    {
        private ILog MockIlog()
        {
            Mock<ILog> protocol = new Mock<ILog>();
            return protocol.Object;
        }
        private BoardManager GetBoardManager()
        {
            ILog log = MockIlog();
            DtService36 db = new DtService36(@"Data Source=shg-l80005754\SQLEXPRESS;Initial Catalog=BMS36;Persist Security Info=True;User ID=sa;Password=cml@shg629;Pooling=False");
            FetchPlans fetchplans = FetchPlans.Inst("");
            fetchplans.FetchPlansList(db);
            Dictionary<string, string> ports = new Dictionary<string, string>();
            ports["OvenPort"] = "COM1";
            ports["Floor1"] = "COM1";
            ports["Floor2"] = "COM2";
            ports["Floor3"] = "COM3";
            ports["Floor4"] = "COM4";
            ports["Floor5"] = "COM5";
            ports["Floor6"] = "COM6";
            ports["Floor7"] = "COM7";
            ports["Floor8"] = "COM8";
            ports["Floor9"] = "COM9";
            ports["Floor10"] = "COM10";
            BoardFactory factroy = new BoardFactory(log, fetchplans, ports);
            SystemParams param = new SystemParams();
            BoardManager manager = new BoardManager(log, db,factroy, param);
            return manager;

        }

        [TestMethod]
        public void Test_CreateInstance()
        {
            BoardManager manager = GetBoardManager();
            manager.LoadBoardUnits();
            Assert.AreNotEqual(manager,null);
        }

        [TestMethod]
        public void Test_SubScribeBoard()
        {
            string boardName = "L10-1";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            BoardUnit uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().BoardName, boardName);
        }
        [TestMethod]
        public void Test_UnsubScribeBoard()
        {
            string boardName = "L10-2";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            manager.UnsubscribeBoard(boardName);
            BoardUnit uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit, null);

        }

        [TestMethod]
        public void Test_GetBoardUnit()
        {
            string boardName1 = "L10-3";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName1, "TEST_TYPE");
            string boardName2 = "L10-4";
            manager.SubscribeBoard(boardName2,"TEST_TYPE");
            BoardUnit uinit = manager.GetBoardUnit(boardName1);
            Assert.AreEqual(uinit.GetBoardInfo().BoardName, boardName1);
            uinit = manager.GetBoardUnit(boardName2);
            Assert.AreEqual(uinit.GetBoardInfo().BoardName, boardName2);
            manager.UnsubscribeBoard(boardName1);
            manager.UnsubscribeBoard(boardName2);
        }

        [TestMethod]
        public void Test_ChangeBoardState()
        {
            string boardName = "L10-3";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            manager.ChangeBoardState(boardName, BoardState.READY);
            List<string> ls = manager.SelectBoardByState(BoardState.READY);
            Assert.AreEqual(ls.Count, 1);
            Assert.AreEqual(ls[0], boardName);
            manager.UnsubscribeBoard(boardName);

        }
        [TestMethod]
        public void Test_AddSeatToBoard()
        {
            string boardName = "L9-1";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            manager.AddSeatToBoard(boardName,1,"001");
            manager.AddSeatToBoard(boardName,2,"002");
            BoardUnit uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().units.Count, 2);
            manager.UnsubscribeBoard(boardName);
        }

        [TestMethod]
        public void Test_AddRepetitiveSnToBoard()
        {
            string boardName = "L9-1";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            manager.AddSeatToBoard(boardName, 1, "001");
            BoardUnit uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().units.Count, 1);

            manager.AddSeatToBoard(boardName, 2, "002");
            uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().units.Count, 2);


            manager.AddSeatToBoard(boardName, 3, "002");
            uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().units.Count, 2);
            manager.UnsubscribeBoard(boardName);
        }

        [TestMethod]
        public void Test_RemoveSeaFromBoard()
        {
            string boardName = "L9-2";
            BoardManager manager = GetBoardManager();
            manager.SubscribeBoard(boardName, "TEST_TYPE");
            manager.AddSeatToBoard(boardName, 1, "001");
            manager.AddSeatToBoard(boardName, 2, "002");
            manager.RemoveSeatFromBoard(boardName,1);
            BoardUnit uinit = manager.GetBoardUnit(boardName);
            Assert.AreEqual(uinit.GetBoardInfo().units.Count, 1);
            manager.UnsubscribeBoard(boardName);
        }
    }


}

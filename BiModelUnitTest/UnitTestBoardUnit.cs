using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BIModel.NowCoding;
using System.Collections.Generic;
using BIModel.Interface;
using BILib;
using BIModel;
using BIModel.Factory;
using BIModel.Manager;


namespace BiModelUnitTest
{

    
    [TestClass]
    public class UnitTestBoardUnit
    {
        private ILog MockIlog()
        {
            Mock<ILog> protocol = new Mock<ILog>();
            return protocol.Object;
        }
        private BoardUnit GetBoardUnit()
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
            BoardUnit unit = new BoardUnit(log, db, param);
            return unit;

        }
        [TestMethod]
        public void BoardUnit_CreateInstance()
        {
        }
    }
}

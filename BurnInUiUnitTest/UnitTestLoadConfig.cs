using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BurnInUI.ConfigReader;
using BIModel.Interface;
namespace BurnInUiUnitTest
{
    [TestClass]
    public class UnitTestLoadConfig
    {
        private ILog MockIlog()
        {
            Mock<ILog> protocol = new Mock<ILog>(); 
            return protocol.Object;
        }
    
        [TestMethod]
        public void Test_LoadConfig()
        {
            LoadConfig load = new LoadConfig(MockIlog());
            ConfigInfo info = load.LoadConfigParam();
            Assert.AreNotEqual(info.dataBasePath, "");
            Assert.AreNotEqual(info.testStation, "");
            Assert.AreEqual(info.boardRows>0 && info.boardRows<=10, true);
            Assert.AreEqual(info.boardColumns>0&&info.boardColumns<=4, true);
            Assert.AreEqual(info.slotColumns >0 && info.slotColumns<=4, true);
            Assert.AreEqual(info.slotRows >0&&info.slotRows<=4, true);
            Assert.AreEqual(info.period >0,true);
            Assert.AreEqual(info.mesCheck, false);
            Assert.AreEqual(info.mbPath, @".\configurations");
            Assert.AreEqual(info.heatTime >= 0, true);
            Assert.AreEqual(info.comErrorTolarence > 0, true);
            Assert.AreEqual(info.conditionTimeout > 0, true);
            Assert.AreEqual(info.floorCount > 0, true);
            Assert.AreNotEqual(info.ovenPort, "");
            Assert.AreNotEqual(info.floor1Port, "");
            Assert.AreNotEqual(info.floor2Port, "");
            Assert.AreNotEqual(info.floor3Port, "");
            Assert.AreNotEqual(info.floor4Port, "");
            Assert.AreNotEqual(info.floor5Port, "");
            Assert.AreNotEqual(info.floor6Port, "");
            Assert.AreNotEqual(info.floor7Port, "");
            Assert.AreNotEqual(info.floor8Port, "");
            Assert.AreNotEqual(info.floor9Port, "");
            Assert.AreNotEqual(info.floor10Port, "");
            Assert.AreEqual(info.mesServiceUrl, "http://localhost:8734/MesApi");
            Assert.AreEqual(info.mesCheck, false);
            Assert.AreEqual(info.mesHoldFlag, false);
            Assert.AreEqual(info.upsSwitch, "OFF");
            Assert.AreEqual(info.upsTimeOut>0, true);
            Assert.AreNotEqual(info.upsPort, "");
        }
         
    }
}

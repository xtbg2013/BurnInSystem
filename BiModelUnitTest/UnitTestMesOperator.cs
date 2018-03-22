using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIModel;
using BIModel.Interface;
namespace BiModelUnitTest
{
    [TestClass]
    public class UnitTestMesOperator
    {
        [TestMethod]
        public void Test_Mes_CreateInstance()
        {
            IMesOperator mes = BiModelFactory.GetMesOperator();
            Assert.AreNotEqual(mes,null);
        }
    }
}

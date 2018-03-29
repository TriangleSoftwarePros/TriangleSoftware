using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VPSci;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Connect()
        {
            bool bRet = false;
            var Class1 = new Shaker();
            bRet = Class1.SetComport("Com1");
        }
    }
}

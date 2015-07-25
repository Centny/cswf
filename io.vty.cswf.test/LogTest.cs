using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace io.vty.cswf.test
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void TestLog()
        {
            ILog log;
            log= LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("error");
            log.Debug("debug");
            log.Info("info");
            log = LogManager.GetLogger("sdfsf");
            log.Fatal("fatal");
            log.Warn("warn");
            log.Debug("end");
        }
    }
}

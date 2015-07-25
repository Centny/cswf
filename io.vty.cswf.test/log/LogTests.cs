using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace io.vty.cswf.log.tests
{
    [TestClass()]
    public class LogTests
    {
        private static readonly ILog L = Log.New();
        private static readonly ILog L2 = Log.New();
        [TestMethod()]
        public void CalssLogTest()
        {
            L.D("testing logging...");
            L2.D("testing logging...");
        }
        [TestMethod()]
        public void InnerLogTest()
        {
            ILog l = Log.New();
            l.Info("sdffdf");
            //
            l.D("sfdsf-{0}-{0}-{1}", 1, 1);
            l.D(new Exception(), "sfdsf-{0}-{0}-{1}", 1, 1);
            l.D("error", new Exception());
            //
            l.I("sfdsf-{0}-{0}-{1}", 1, 1);
            l.I(new Exception(), "sfdsf-{0}-{0}-{1}", 1, 2);
            l.I("error", new Exception());
            //
            l.W("sfdsf-{0}-{0}-{1}", 1, 1);
            l.W(new Exception(), "sfdsf-{0}-{0}-{1}", 1, 3);
            l.W("error", new Exception());
            //
            l.E("sfdsf-{0}-{0}-{1}", 1, 1);
            l.E(new Exception(), "sfdsf-{0}-{0}-{1}", 1, 4);
            l.E("error", new Exception());
            //
            l.F("sfdsf-{0}-{0}-{1}", 1, 1);
            l.F(new Exception(), "sfdsf-{0}-{0}-{1}", 1, 5);
            l.F("error", new Exception());
            //
            Console.WriteLine("sfdsfd");
        }
        [TestMethod()]
        public void ListLogTest()
        {
            Log.GetLogger("abc");
            Log.GetLogger(Assembly.GetCallingAssembly(), "abc");
            log4net.Repository.ILoggerRepository rep = log4net.LogManager.GetRepository();
            Log.GetLogger(rep.Name, "abc");
            Log.GetLogger(rep.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Log.GetLogger(typeof(LogTests));
            Log.GetLogger(Assembly.GetCallingAssembly(), typeof(LogTests));
            Assert.IsNotNull(Log.Exists(Assembly.GetCallingAssembly(), "abc"));
            Assert.IsNotNull(Log.Exists("abc"));
            Assert.IsNotNull(Log.Exists(rep.Name, "abc"));
            Assert.IsTrue(Log.GetCurrentLoggers().Length > 0);
            Assert.IsTrue(Log.GetCurrentLoggers(Assembly.GetCallingAssembly()).Length > 0);
            Assert.IsTrue(Log.GetCurrentLoggers(rep.Name).Length > 0);
        }
    }
}
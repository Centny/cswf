using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace io.vty.cswf.test.log
{
    public class TestLog
    {
        private static ILog L = Log.New();

        public static void show()
        {
            L.D("doing debug...");
        }
    }
}

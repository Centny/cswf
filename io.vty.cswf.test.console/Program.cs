using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace io.vty.cswf.test.console
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestLog.show();
            //ILog xx =Log.New();
            //xx.D("xxxss");
            /*
            ILog log;
            log = LogManager.GetLogger("ssss");
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("sddfsf");
           // log.Debug("xxxxkkk");
            */
            //Console.WriteLine("sdsf");
        }
    }
}

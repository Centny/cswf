using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace io.vty.cswf.test.console
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log;
            log = LogManager.GetLogger("ssss");
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error("sddfsf");
            Console.WriteLine("sdsf");
        }
    }
}

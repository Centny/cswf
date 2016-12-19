using io.vty.cswf.log;
using io.vty.cswf.netw.http;
using io.vty.cswf.netw.http.filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace io.vty.cswf.test.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var srv = new Server();
            srv.AddPrefix("http://localhost:9233/");
            srv.AddF(".*", new CORS().Exec);
            srv.AddH(".*", r =>
             {
                 r.WriteLine("abcv");
                 return HResult.HRES_RETURN;
             });
            srv.Start();
            Thread.Sleep(1000000);
        }
    }
}

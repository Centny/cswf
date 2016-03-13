using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw.http;
using System.IO;
using io.vty.cswf.util;

namespace io.vty.cswf.test.netw.http
{
    [TestClass]
    public class ServeTest
    {
        public HResult testa(Request r)
        {
            StreamWriter sw = new StreamWriter(r.res.OutputStream);
            sw.WriteLine("OK");
            sw.Flush();
            return HResult.HRES_RETURN;
        }
        [TestMethod]
        public void TestServer()
        {
            var srv = new Server();
            srv.AddPrefix("http://localhost:1234/");
            srv.AddF("^/testa(\\?.*)?$", this.testa);
            srv.Start();

            var res = H.doGet("http://localhost:1234/testa");
            Console.WriteLine("->" + res.Data);
            srv.Stop();
        }
    }
}

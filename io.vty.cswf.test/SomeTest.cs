using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using io.vty.cswf.util;
using System.Threading;
using io.vty.cswf.io;
using io.vty.cswf.netw;

namespace io.vty.cswf.test
{
    [TestClass]
    public class SomeTest
    {

        [TestMethod]
        public void Stream2Test()
        {
            var ms = new MemoryStream();
            Console.WriteLine(ms.CanWrite);
            Console.WriteLine(ms.CanRead);
            ms.Write(new byte[3] { 1, 2, 3 }, 0, 3);
            ms.Flush();

            var bys = new byte[1024];
            var len = ms.Read(bys, 0, 1024);
            Console.WriteLine(len + "->" + BysImplV.bstr(bys));
        }
    }
}

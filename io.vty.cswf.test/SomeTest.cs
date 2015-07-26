using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using io.vty.cswf.test.util;
using System.Threading;
using io.vty.cswf.io;

namespace io.vty.cswf.test
{
    [TestClass]
    public class SomeTest
    {
        [TestMethod]
        public void TestStream()
        {
            var running = true;
            var ms = new PipeStream();
            var bys = Util.bytes("abc\n");
            new Task<int>(() =>
            {
                var buf = new byte[4];
                while (running)
                {
                    var len = ms.Read(buf, 0, 4);
                    Console.Write("R-({1})>{0}", Util.tos(buf), len);
                }
                Console.WriteLine("R->end");
                return 0;
            }, 0).Start();
            new Task<int>(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("W->" + i);
                    ms.Write(bys, 0, bys.Length);
                    ms.Flush();
                }
                return 0;
            }, 0).Start();
            Thread.Sleep(500);
            running = false;
            ms.Write(bys, 0, bys.Length);
            Thread.Sleep(500);
        }
    }
}

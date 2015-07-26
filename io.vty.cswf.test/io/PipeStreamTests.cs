using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.util;
using System.Threading;

namespace io.vty.cswf.io.tests
{
    [TestClass()]
    public class PipeStreamTests
    {
        [TestMethod()]
        public void PipeStreamTest()
        {
            var running = true;
            var ms = new PipeStream(100);
            new Task<int>(() =>
            {
                try
                {
                    var buf = new byte[1024];
                    while (running)
                    {
                        var len = ms.Read(buf, 0, 1024);
                        Console.Write("R-({1})>{0}\n", Util.tos(buf, 0, len), len);
                    }
                }
                catch (Exception e)
                {

                }
                Console.WriteLine("R->end");
                return 0;
            }, 0).Start();
            new Task<int>(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("W->" + i);
                    var bys = Util.bytes("abc" + i);
                    ms.Write(bys, 0, bys.Length);
                    //ms.Flush();
                }
                return 0;
            }, 0).Start();
            Thread.Sleep(1000);
            running = false;
            ms.Close();
            Thread.Sleep(500);
        }

    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace io.vty.cswf.util.tests
{
    [TestClass()]
    public class CDLTests
    {
        [TestMethod()]
        public void CDLTest()
        {
            var cdl = new CDL(100);
            for (int i = 0; i < 100; i++)
            {
                new Task<int>(() =>
                {
                    Thread.Sleep(10);
                    cdl.done();
                    return 0;
                }).Start();
            }
            cdl.wait(50);
            Assert.IsTrue(cdl.current >= 50);
            Console.WriteLine("50->");
            cdl.wait();
            Assert.IsTrue(cdl.current == 100);
            Console.WriteLine("done->");
        }

    }
}
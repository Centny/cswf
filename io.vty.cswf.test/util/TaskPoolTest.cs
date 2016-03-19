using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System.Threading;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class TaskPoolTest
    {
        public int count = 0;
        [TestMethod]
        public void TestTaskPool()
        {
            TaskPool.Shared.MaximumConcurrency = 3;
            bool fail = false;
            var cdl = new CDL(20);
            for(var i = 0; i < 20; i++)
            {
                TaskPool.Queue(t =>
                {
                    fail=Interlocked.Increment(ref this.count)>3;
                    Thread.Sleep(1000);
                    Interlocked.Decrement(ref this.count);
                    cdl.done();
                }, i);
            }
            cdl.wait();
            Assert.AreEqual(false, fail);
        }
    }
}

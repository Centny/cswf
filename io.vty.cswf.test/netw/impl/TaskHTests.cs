using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;
using System.Threading;

namespace io.vty.cswf.netw.impl.tests
{
    [TestClass()]
    public class TaskHTests : r.CmdListener
    {
        private int val = 0;
        public void onCmd(NetwRunnable nr, Bys m)
        {
            Console.WriteLine(val++);
            Thread.Sleep(300);
        }

        [TestMethod()]
        public void onCmdTest()
        {
            Console.WriteLine("starting...\n");
            TaskH h = new TaskH(this);
            for (int i = 0; i < 100; i++)
            {
                h.onCmd(null, null);
            }
            Thread.Sleep(5000);
            Console.WriteLine("running->" + val);
        }
    }
}
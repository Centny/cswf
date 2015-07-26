using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl.tests
{
    [TestClass()]
    public class QueueHTests
    {
        private int val = 0;
        public class H : r.CmdListener
        {
            private QueueHTests t;
            private int val;
            public H(QueueHTests t, int v)
            {
                this.t = t;
                this.val = v;
            }
            public void onCmd(NetwRunnable nr, Bys m)
            {
                Assert.AreEqual(this.val, this.t.val);
                this.t.val++;
            }
        }
        [TestMethod()]
        public void onCmdTest()
        {
            QueueH h = new QueueH();
            h.addh(new H(this, 0));
            h.addh(new H(this, 1));
            h.addh(new H(this, 2));
            h.addh(new H(this, 3));
            h.onCmd(null, null);
            Assert.AreEqual(this.val, 4);
        }

    }
}
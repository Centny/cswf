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
    public class OBDHTests
    {
        public class H : r.CmdListener
        {
            public byte val;
            public H(byte val)
            {
                this.val = val;
            }
            public void onCmd(NetwRunnable nr, Bys m)
            {
                Assert.AreEqual(this.val, m.get(0));
            }
        }
        [TestMethod()]
        public void onCmdTest()
        {
            OBDH h = new OBDH();
            h.addh(0, new H(1));
            h.addh(1, new H(2));
            h.addh(2, new H(3));
            h.onCmd(null, new BysImplV(null, new byte[2] { 0, 1 }));
            h.onCmd(null, new BysImplV(null, new byte[2] { 1, 2 }));
            h.onCmd(null, new BysImplV(null, new byte[2] { 2, 3 }));
            //not found
            h.onCmd(null, new BysImplV(null, new byte[2] { 3, 4 }));
        }
    }
}
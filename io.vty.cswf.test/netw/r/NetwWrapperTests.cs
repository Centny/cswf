using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.io;
using io.vty.cswf.util;

namespace io.vty.cswf.netw.r.tests
{
    [TestClass()]
    public class NetwWrapperTests
    {

        public class NetwImpl_T : NetwImpl
        {
            public NetwImpl_T(NetwBase rw) : base(rw)
            {

            }
            public override Bys newM(byte[] m, int off, int len)
            {
                return new BysImpl(this, m, off, len);
            }
        }
        [TestMethod()]
        public void WrapperTests()
        {
            NetwWrapper nw = new NetwWrapper(new NetwImpl_T(new NetwBaseImpl(new PipeStream(1024), 102400)));
            byte[] tmp;
            byte[] bys1 = new byte[] { 1, 2, 3, 4 };
            byte[] bys2 = new byte[] { 4, 3, 2, 1 };
            byte[] bys3 = new byte[] { 1, 2, 3, 4, 4, 3, 2, 1 };
            //
            nw.writem(bys1);
            tmp = nw.readm();
            Assert.AreEqual(true, Util.equal(bys1, tmp));
            //
            nw.writem(bys1, 0, 2);
            tmp = nw.readm();
            Assert.AreEqual(true, Util.equal(new byte[] { 1, 2 }, tmp));
            //
            IList<byte[]> bs = new List<byte[]>();
            bs.Add(bys1);
            bs.Add(bys2);
            nw.writem(bs);
            tmp = nw.readm();
            Assert.AreEqual(true, Util.equal(bys3, tmp));
            //
            //
            Bys bs1 = new BysImpl(null, bys1);
            Bys tm2;
            //
            nw.writeM(bs1);
            tm2 = nw.readM();
            Assert.AreEqual(true, tm2.Equals(bs1));
            //
            IList<Bys> bs2 = new List<Bys>();
            bs2.Add(bs1);
            nw.writeM(bs2);
            tm2 = nw.readM();
            Assert.AreEqual(true, tm2.Equals(bs1));
            //
            tmp = new byte[bys1.Length + 5];
            nw.writem(bys1);
            nw.readw(tmp);
            Assert.AreEqual(true, bys1[0] == tmp[5]);
            bys1[bys1.Length - 1] = 11;
            nw.writem(bys1);
            nw.readw(tmp, 0, tmp.Length);
            Assert.AreEqual(true, bys1[0] == tmp[5]);
        }
    }
}
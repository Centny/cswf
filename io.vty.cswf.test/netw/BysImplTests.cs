using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.tests
{
    [TestClass()]
    public class BysImplTests
    {
        [TestMethod()]
        public void resetTest()
        {
            BysImpl bys = new BysImpl(null, new byte[10]);
            bys.reset(1, 5);
            byte[] bys2 = bys.sbys;
            Assert.AreEqual(5, bys2.Length);
            bys.reset(0, 0);
            bys.toBs();
            //for cover
            try
            {
                bys.reset(-1, 0);
            }
            catch (Exception e)
            {

            }
            try
            {
                bys.V<Object>();
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod()]
        public void shortvTest()
        {
            BysImpl bys = new BysImpl(null, new byte[] { 0, 2, 1, 2 });
            Assert.AreEqual(2, bys.shortv(0));
            Assert.AreEqual(258, bys.shortv(2));
        }

        public class NetwImplT : NetwImpl
        {
            public NetwImplT(NetwBase rwb) : base(rwb)
            {
            }
            public override Bys newM(byte[] m, int off, int len)
            {
                return new BysImpl(this, m, off, len);
            }
        }
        [TestMethod()]
        public void sliceTest()
        {
            BysImpl bys = new BysImpl(new NetwImplT(null), new byte[] { 0, 1, 2, 3, 4 });
            Bys bys1 = bys.slice(3);
            Bys bys2 = bys.slice(2, 2);
            Assert.AreEqual(2, bys1.length);
            Assert.AreEqual(2, bys2.length);
            Assert.AreEqual(3 * 256 + 4, bys1.shortv(0));
            Assert.AreEqual(2 * 256 + 3, bys2.shortv(0));
            try
            {
                bys.slice(1000);
            }
            catch (Exception e)
            {

            }
            try
            {
                bys.slice(1000, 1000);
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Bys bys1 = new BysImpl(null, new byte[] { 1, 2, 3 });
            Bys bys2 = new BysImpl(null, new byte[] { 1, 2, 4 });
            Bys bys3 = new BysImpl(null, new byte[] { 1, 2, 3, 4 });
            Assert.AreEqual(false, bys1.Equals(null));
            Assert.AreEqual(false, bys2.Equals(bys1));
            Assert.AreEqual(false, bys3.Equals(bys1));
        }
    }
}
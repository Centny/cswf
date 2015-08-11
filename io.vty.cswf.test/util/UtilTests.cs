using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util.tests
{
    [TestClass()]
    public class UtilTests
    {
        [TestMethod()]
        public void tosTest()
        {
            Assert.AreEqual("abc", Util.tos(Util.bytes("abc")));
        }

        [TestMethod()]
        public void equalTest()
        {
            byte[] bys1 = new byte[] { 1, 2, 3 };
            byte[] bys2 = new byte[] { 1, 2, 4 };
            byte[] bys3 = new byte[] { 1, 2, 3, 4 };
            Assert.AreEqual(false, Util.equal(bys1, bys2));
            Assert.AreEqual(false, Util.equal(bys1, bys3));
            Assert.AreEqual(true, Util.equal(null, null));
        }
    }
}
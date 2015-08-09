using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            try
            {
                bys.reset(-1, 0);
            }catch(Exception e)
            {

            }
        }
    }
}
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
    public class NetwImplTests
    {
        public class NetwImpl_ : NetwImpl
        {
            public NetwImpl_(NetwBase rwb) : base(rwb)
            {
            }
            public override Bys newM(NetwBase rw, byte[] m, int off, int len)
            {
                throw new NotImplementedException();
            }
        }
        [TestMethod()]
        public void writemTest()
        {
            NetwImpl_ nw = new NetwImpl_(null);
            try
            {
                IList<byte[]> bys = null;
                nw.writem(bys);
            }
            catch (Exception)
            {

            }
            //try
            //{
            //    cmds = new List<Bys>();
            //    cmds.Add(new BysImpl(null, new byte[10240]));
            //    r.strc.limit = 100;
            //    r.strc.writeM(cmds);
            //}
            //catch (Exception e)
            //{

            //}
        }
    }
}
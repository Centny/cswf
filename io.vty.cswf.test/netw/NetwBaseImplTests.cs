using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.tests
{
    [TestClass()]
    public class NetwBaseImplTests
    {
        public class NetwBaseImpl_ : NetwBaseImpl
        {
            public NetwBaseImpl_(Stream stream, int limit) : base(stream, limit)
            {

            }
        }
        [TestMethod()]
        public void writeMTest()
        {
            NetwBaseImpl_ nb = new NetwBaseImpl_(null, 10);

            IList<Bys> bys = null;
            try
            {
                bys = null;
                nb.writeM(bys);
            }catch(Exception e)
            {

            }
            try
            {
                bys = new List<Bys>();
                bys.Add(new BysImpl(null, new byte[1024]));
                nb.writeM(bys);
            }
            catch (Exception e)
            {

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers impl writev and newM for NetwVer.
    /// </summary>
    public abstract class NetwImplV : NetwImpl, r.NetwVer
    {
        /// <summary>
        /// the construcotr by base stream.
        /// </summary>
        /// <param name="rwb"></param>
        public NetwImplV(NetwBase rwb) : base(rwb)
        {
        }

        public abstract T B2V<T>(Bys bys);

        public abstract Bys V2B(Netw nv, object v);

        public void writev(object v)
        {
            this.writeM(this.V2B(this, v));
        }

        public override Bys newM(byte[] m, int off, int len)
        {
            return new BysImplV(this, m, off, len);
        }
    }
}

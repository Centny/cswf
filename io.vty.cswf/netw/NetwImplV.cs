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
    public abstract class NetwImplV : NetwImpl, r.NetwVer, Converter
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

        public void writev(Bys bys, object v)
        {
            this.writeM(bys, this.V2B(this, v));
        }

        public override Bys newM(byte[] m, int off, int len)
        {
            return new BysImplV(this, this, m, off, len);
        }

        public override Bys newM(NetwBase rw, byte[] m, int off, int len)
        {
            return new BysImplV(new Wrapper(this, rw), this, m, off, len);
        }

        public class Wrapper : NetwImplV
        {
            public r.Converter v;
            public Wrapper(r.Converter v, NetwBase rwb) : base(rwb)
            {
                this.v = v;
            }
            public override T B2V<T>(Bys bys)
            {
                return this.v.B2V<T>(bys);
            }

            public override Bys V2B(Netw nv, object v)
            {
                return this.v.V2B(nv, v);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers impl V function.
    /// </summary>
    public class BysImplV : BysImpl
    {
        /// <summary>
        /// the base stream.
        /// </summary>
        protected Converter ver { get; set; }
        public BysImplV(Netw rw, Converter ver, byte[] bys) : base(rw, bys)
        {
            this.ver = ver;
        }

        public BysImplV(Netw rw, Converter ver, byte[] bys, int off, int len) : base(rw, bys, off, len)
        {
            this.ver = ver;
        }

        public override T V<T>()
        {
            return this.ver.B2V<T>(this);
        }
        public override void writev(object v)
        {
            List<Bys> bys = new List<Bys>();
            bys.Add(this.ver.V2B(this, v));
            this.writeM(bys);
        }
    }
}

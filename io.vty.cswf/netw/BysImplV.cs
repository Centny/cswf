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
        protected NetwVer ver { get; set; }
        public BysImplV(NetwVer ver, byte[] bys) : base(ver, bys)
        {
            this.ver = ver;
        }

        public BysImplV(NetwVer ver, byte[] bys, int off, int len) : base(ver, bys, off, len)
        {
            this.ver = ver;
        }

        public override T V<T>()
        {
            return this.ver.B2V<T>(this);
        }
    }
}

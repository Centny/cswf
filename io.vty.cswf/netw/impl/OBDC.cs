using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers one byte distribute connection.
    /// </summary>
    public class OBDC : r.NetwBase
    {
        /// <summary>
        /// default constructor by base stream and byte.
        /// </summary>
        /// <param name="nb">base stream</param>
        /// <param name="m">byte mode</param>
        public OBDC(r.NetwBase nb, byte m)
        {
            this.nb = nb;
            this.m = m;
        }
        /// <summary>
        /// one byte mode.
        /// </summary>
        protected byte m { get; set; }

        /// <summary>
        /// the base stream.
        /// </summary>
        protected NetwBase nb { get; set; }

        public int limit
        {
            get
            {
                return this.nb.limit;
            }

            set
            {
                this.nb.limit = value;
            }
        }

        public int readw(byte[] buf, int off, int len)
        {
            return this.nb.readw(buf, off, len);
        }

        public void writeM(IList<Bys> ms)
        {
            List<Bys> tms = new List<Bys>();
            tms.Add(new BysImpl(null, new byte[1] { this.m }, 0, 1));
            tms.AddRange(ms);
            this.nb.writeM(tms);
        }
    }
}

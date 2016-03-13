using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers one byte distribute connection.
    /// </summary>
    public class PrefC : r.NetwBase
    {
        public virtual Stream stream
        {
            get
            {
                return this.nb.stream;
            }

            set
            {
                this.nb.stream = value;
            }
        }
        /// <summary>
        /// one byte mode.
        /// </summary>
        protected virtual Bys pref { get; set; }

        /// <summary>
        /// the base stream.
        /// </summary>
        protected virtual NetwBase nb { get; set; }

        public virtual int limit
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

        /// <summary>
        /// default constructor by base stream and byte.
        /// </summary>
        /// <param name="nb">base stream</param>
        /// <param name="m">byte mode</param>
        public PrefC(r.NetwBase nb, Bys pref)
        {
            this.nb = nb;
            this.pref = pref;
        }
        public PrefC(r.NetwBase nb, byte pref)
        {
            this.nb = nb;
            this.pref = new BysImpl(null, new byte[1] { pref }, 0, 1);
        }

        public virtual int readw(byte[] buf, int off, int len)
        {
            return this.nb.readw(buf, off, len);
        }

        public virtual void writeM(IList<Bys> ms)
        {
            List<Bys> tms = new List<Bys>();
            tms.Add(this.pref);
            tms.AddRange(ms);
            this.nb.writeM(tms);
        }
    }
}

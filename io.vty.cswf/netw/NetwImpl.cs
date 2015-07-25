using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;
using System.IO;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers base impl to Netw.
    /// </summary>
    public abstract class NetwImpl : r.Netw
    {
        /// <summary>
        /// the base stream.
        /// </summary>
        protected virtual NetwBase rwb { get; set; }
        /// <summary>
        /// the constructor by base stream.
        /// </summary>
        /// <param name="rwb"></param>
        public NetwImpl(NetwBase rwb)
        {
            this.rwb = rwb;
        }
        public virtual int limit
        {
            get
            {
                return this.rwb.limit;
            }

            set
            {
                this.rwb.limit = value;
            }
        }

        public abstract Bys newM(byte[] m, int off, int len);

        public Bys readM()
        {
            byte[] m = this.readm();
            return this.newM(m, 0, m.Length);
        }

        public byte[] readm()
        {
            throw new NotImplementedException();
        }

        public int readw(byte[] buf)
        {
            return this.readw(buf,0,buf.Length);
        }

        public int readw(byte[] buf, int off, int len)
        {
            return this.rwb.readw(buf, off, len);
        }

        public void writem(IList<byte[]> ms)
        {
            if (ms == null || ms.Count < 1)
            {
                throw new InvalidDataException("the data list is null or empty");
            }
            IList<Bys> tms = new List<Bys>();
            foreach(byte[] m in ms)
            {
                tms.Add(this.newM(m, 0, m.Length));
            }
            this.writeM(tms);
        }

        public void writeM(IList<Bys> ms)
        {
            this.rwb.writeM(ms);
        }

        public void writeM(Bys m)
        {
            IList<Bys> tms = new List<Bys>();
            tms.Add(m);
            this.writeM(tms);
        }

        public void writem(byte[] m)
        {
            this.writem(m, 0, m.Length);
        }

        public void writem(byte[] m, int off, int length)
        {
            this.writeM(this.newM(m, off, length));
        }
    }
}

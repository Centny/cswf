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
        private readonly byte[] hbuf = new byte[5];
        public Stream stream
        {
            get
            {
                return this.rwb.stream;
            }

            set
            {
                this.rwb.stream = value;
            }
        }
        /// <summary>
        /// the base stream.
        /// </summary>
        public virtual NetwBase rwb { get; protected set; }
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

        public virtual Bys newM(byte[] m, int off, int len)
        {
            return this.newM(this, m, off, len);
        }
        public abstract Bys newM(NetwBase rw, byte[] m, int off, int len);

        public Bys readM()
        {
            byte[] m = this.readm();
            return this.newM(m, 0, m.Length);
        }

        public byte[] readm()
        {
            this.readw(this.hbuf);
            int len = 0;
            len += (this.hbuf[3]) << 8;
            len += (this.hbuf[4]);
            if (!Var.valid_h(this.hbuf, 0) || len < 1)
            {
                throw new ModException("reading invalid mode for data:" + BysImpl.bstr(this.hbuf));
            }
            byte[] tbuf = new byte[len];
            this.readw(tbuf);
            return tbuf;
        }

        public int readw(byte[] buf)
        {
            return this.readw(buf, 0, buf.Length);
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
            foreach (byte[] m in ms)
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

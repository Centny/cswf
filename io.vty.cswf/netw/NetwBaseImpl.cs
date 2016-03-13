using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers impl to NetwBase.
    /// </summary>
    public class NetwBaseImpl : NetwBase
    {
        /// <summary>
        /// the base stream.
        /// </summary>
        public virtual Stream stream { get; set; }
        /// <summary>
        /// the write limit.
        /// </summary>
        public virtual int limit { get; set; }

        protected readonly byte[] wbuf = new byte[5];

        /// <summary>
        /// the default stream by base stream.
        /// </summary>
        /// <param name="stream">base stream</param>
        /// <param name="limit">limit</param>
        public NetwBaseImpl(Stream stream, int limit = 1024000)
        {
            this.stream = stream;
            this.limit = limit;
            Buffer.BlockCopy(Var.H_MOD, 0, this.wbuf, 0, 3);
        }

        public virtual int readw(byte[] buf, int off, int len)
        {
            int rlen = 0;
            int toff = off;
            int tlen = len;
            while (tlen > 0)
            {
                rlen = this.stream.Read(buf, toff, tlen);
                if (rlen < 1)
                {
                    throw new EOFException();
                }
                toff += rlen;
                tlen -= rlen;
                rlen = 0;
            }
            return len;
        }

        public virtual void writeM(IList<Bys> ms)
        {
            if (ms == null || ms.Count < 1)
            {
                throw new InvalidDataException("data list is null or empty");
            }
            lock (this)
            {
                int len = 0;
                foreach (Bys m in ms)
                {
                    len += m.length;
                }
                if (len > this.limit)
                {
                    throw new IOException("message too large, must less " + this.limit + ", but " + len);
                }
                this.wbuf[3] = (byte)(len >> 8);
                this.wbuf[4] = (byte)len;
                this.stream.Write(this.wbuf, 0, 5);
                foreach (Bys m in ms)
                {
                    this.stream.Write(m.bys, m.offset, m.length);
                }
                this.stream.Flush();
            }
        }
    }
}

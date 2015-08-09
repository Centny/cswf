using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers wrapper to base Netw.
    /// </summary>
    public class NetwWrapper : Netw
    {
        public virtual Stream stream
        {
            get
            {
                return this.bnw.stream;
            }

            set
            {
                this.bnw.stream = stream;
            }
        }
        /// <summary>
        /// base Netw stream.
        /// </summary>
        public virtual Netw bnw { get; set; }
        /// <summary>
        /// the default constructor by base Netw stream.
        /// </summary>
        /// <param name="nw">base stream</param>
        public NetwWrapper(Netw nw)
        {
            this.bnw = nw;
        }
        public virtual int limit
        {
            get
            {
                return bnw.limit;
            }

            set
            {
                bnw.limit = value;
            }
        }

        public virtual Bys newM(byte[] m, int off, int len)
        {
            return bnw.newM(m, off, len);
        }

        public virtual Bys readM()
        {
            return bnw.readM();
        }

        public virtual byte[] readm()
        {
            return bnw.readm();
        }

        public virtual int readw(byte[] buf)
        {
            return bnw.readw(buf);
        }

        public virtual int readw(byte[] buf, int off, int len)
        {
            return bnw.readw(buf, off, len);
        }

        public virtual void writem(IList<byte[]> ms)
        {
            bnw.writem(ms);
        }

        public virtual void writeM(IList<Bys> ms)
        {
            bnw.writeM(ms);
        }

        public virtual void writeM(Bys m)
        {
            bnw.writeM(m);
        }

        public virtual void writem(byte[] m)
        {
            bnw.writem(m);
        }

        public virtual void writem(byte[] m, int off, int length)
        {
            bnw.writem(m, off, length);
        }
    }
}

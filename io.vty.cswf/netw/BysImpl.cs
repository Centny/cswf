using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers base impl to Bys.
    /// </summary>
    public class BysImpl : r.NetwWrapper, r.Bys
    {
        public virtual byte[] bys { get; private set; }

        public virtual int length { get; private set; }

        public virtual int offset { get; private set; }

        public BysImpl(Netw nw, byte[] bys) : base(nw)
        {
            this.bys = bys;
            this.offset = 0;
            this.length = bys.Length;
        }
        public BysImpl(Netw nw, byte[] bys, int off, int len) : base(nw)
        {
            this.bys = bys;
            this.offset = off;
            this.length = len;
        }

        public virtual byte[] sbys
        {
            get
            {
                byte[] buf = new byte[this.length];
                Buffer.BlockCopy(this.bys, this.offset, buf, 0, this.length);
                return buf;
            }
        }

        public virtual void forward(int len)
        {
            this.offset += len;
            this.length -= len;
        }

        public virtual byte get(int idx)
        {
            return this.bys[this.offset + idx];
        }

        public virtual void reset(int off, int len)
        {
            if (off < 0 || off >= this.bys.Length || len < 0 || len > this.bys.Length || off + len > this.bys.Length)
            {
                throw new IndexOutOfRangeException();
            }
            this.offset = off;
            this.length = len;
        }

        public virtual short shortv(int off)
        {
            int val = 0;
            val += this.bys[this.offset + off] << 8;
            val += this.bys[this.offset + off + 1];
            return (short)val;
        }

        public virtual Bys slice(int off)
        {
            if (off >= this.length)
            {
                throw new IndexOutOfRangeException();
            }
            return this.slice(off, this.length - off);
        }

        public virtual Bys slice(int off, int len)
        {
            if (off + len > this.length)
            {
                throw new IndexOutOfRangeException();
            }
            return this.newM(this.bys, this.offset + off, len);
        }

        public virtual string toBs()
        {
            return bstr(this.bys, this.offset, this.length);
        }

        public override string ToString()
        {
            return new string(Encoding.Default.GetChars(this.bys, this.offset, this.length));
        }

        public virtual T V<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// convert byte[] to like:[1,3,4];
        /// </summary>
        /// <param name="bys">byte[]</param>
        /// <returns></returns>
        public static string bstr(byte[] bys)
        {
            return bstr(bys, 0, bys.Length);
        }
        /// <summary>
        /// convert byte[] to like:[1,2,3]
        /// </summary>
        /// <param name="bys">byte[]</param>
        /// <param name="off">data offset</param>
        /// <param name="len">data length</param>
        /// <returns></returns>
        public static string bstr(byte[] bys, int off, int len)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            if (bys.Length > 0 && off < bys.Length && off > -1 && len > 0 && bys.Length >= len)
            {
                sb.Append(bys[off]);
                for (int i = off + 1; i < off + len; i++)
                {
                    sb.Append(',');
                    sb.Append(bys[i]);
                }
            }
            sb.Append(']');
            return sb.ToString();
        }

    }
}

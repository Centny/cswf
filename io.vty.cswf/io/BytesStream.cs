using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace io.vty.cswf.io
{
    /// <summary>
    /// PipeStream is a thread-safe read/write data stream for use between two threads in a 
    /// single-producer/single-consumer type problem.
    /// </summary>
    public class BytesStream : Stream
    {
        protected virtual byte[] buf { get; set; }
        protected virtual int off { get; set; }
        protected virtual int len { get; set; }
        public virtual bool IsClosed { get; protected set; }

        public BytesStream(int buf)
        {
            this.buf = new byte[buf];
            this.off = 0;
            this.len = 0;
        }
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return this.len;
            }
        }

        public override long Position
        {
            get
            {
                return 0;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (this.buf)
            {
                if (this.len < 1)
                {
                    Monitor.Wait(this.buf);
                }
                if (this.IsClosed)
                {
                    return 0;
                }
                var tc = count;
                if (this.len < tc)
                {
                    tc = this.len;
                }
                if (this.off + tc > this.buf.Length)
                {
                    Buffer.BlockCopy(this.buf, this.off, buffer, offset, this.buf.Length - this.off);
                    Buffer.BlockCopy(this.buf, 0, buffer, offset + this.buf.Length - this.off, tc + this.off - this.buf.Length);
                }
                else
                {
                    Buffer.BlockCopy(this.buf, this.off, buffer, offset, tc);
                }
                this.len -= tc;
                if (this.off + tc < this.buf.Length)
                {
                    this.off += tc;
                }
                else
                {
                    this.off = this.off + tc - this.buf.Length;
                }
                Monitor.Pulse(this.buf);
                return tc;
            }
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (this)
            {
                var wc = 0;
                while (wc < count)
                {
                    wc += this.Write_(buffer, offset + wc, count - wc);
                }
            }
        }

        /// <summary>
        /// write data to stream and return writed count.
        /// </summary>
        /// <param name="buffer">buffer byte[]</param>
        /// <param name="offset">data offset.</param>
        /// <param name="count">data count.</param>
        /// <returns></returns>
        private int Write_(byte[] buffer, int offset, int count)
        {
            lock (this.buf)
            {
                if (this.len >= this.buf.Length)
                {
                    Monitor.Wait(this.buf);
                }
                if (this.IsClosed)
                {
                    throw new IOException();
                }
                var tc = count;
                if (tc > this.buf.Length - this.len)
                {
                    tc = this.buf.Length - this.len;
                }
                if (this.off + this.len + tc > this.buf.Length)
                {
                    if (this.off + this.len < this.buf.Length)
                    {
                        Buffer.BlockCopy(buffer, offset, this.buf, this.off + this.len, this.buf.Length - this.off - this.len);
                        Buffer.BlockCopy(buffer, offset + this.buf.Length - this.off - this.len, this.buf, 0, tc - (this.buf.Length - this.off - this.len));
                    }
                    else
                    {
                        Buffer.BlockCopy(buffer, offset, this.buf, this.off + this.len - this.buf.Length, tc);
                    }
                }
                else
                {
                    Buffer.BlockCopy(buffer, offset, this.buf, this.off + this.len, tc);
                }
                this.len += tc;
                Monitor.Pulse(this.buf);
                return tc;
            }
        }

        public override void Close()
        {
            base.Close();
            lock (this.buf)
            {
                this.IsClosed = true;
                Monitor.Pulse(this.buf);
            }

        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

    }
}

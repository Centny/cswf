using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.io
{
    public class PipeStream : BytesStream
    {
        public PipeStream Side;
        private PipeStream(int buf) : base(buf)
        {
        }
        protected virtual void BaseWrite(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.Side.BaseWrite(buffer, offset, count);
        }

        public static PipeStream create(int buf)
        {
            PipeStream left = new PipeStream(buf);
            PipeStream right = new PipeStream(buf);
            left.Side = right;
            right.Side = left;
            return left;
        }
    }
}

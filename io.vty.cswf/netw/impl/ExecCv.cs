using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.netw.impl
{
    public class ExecCv : ExecC
    {
        protected Converter c;
        public ExecCv(Netw rw, Converter c) : base(rw)
        {
            this.c = c;
        }
        public void exec(byte m, Object args, CmdListener l)
        {
            base.exec(m, this.c.V2B(this.rw, args), l);
        }

        public Bys exec(byte m, Object args)
        {
            return base.exec(m, this.c.V2B(this.rw, args));
        }

        public string exec_s(byte m, Object args)
        {
            Bys bys = this.exec(m, args);
            return bys.ToString();
        }

        public T exec<T>(byte m, Object args)
        {
            return base.exec<T>(m, this.c.V2B(this.rw, args));
        }
        public IDictionary<string, Object> exec_m(byte m, Object args)
        {
            return this.exec<Dictionary<string, Object>>(m, args);
        }
        public T exec<T>(Object args)
        {
            return this.exec<T>(0, args);
        }
        public Bys exec(Object args)
        {
            return this.exec(0, args);
        }
        public string exec_s(Object args)
        {
            return this.exec_s(0, args);
        }
        public IDictionary<string, Object> exec_m(Object args)
        {
            return this.exec<Dictionary<string, Object>>(0, args);
        }
    }
}

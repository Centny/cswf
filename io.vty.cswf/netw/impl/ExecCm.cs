using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.netw.impl
{
    public class ExecCm : ExecCv
    {
        public static Object V_NAV(ExecCm rc, string name, Object args)
        {
            IDictionary<string, Object> data = new Dictionary<string, Object>();
            data.Add("args", args);
            data.Add("name", name);
            return data;
        }
        public delegate Object NAV_F(ExecCm rc, string name, Object args);
        protected NAV_F nav;
        public ExecCm(Netw rw, Converter c, NAV_F nav) : base(rw, c)
        {
            this.nav = nav;
        }
        public ExecCm(Netw rw, Converter c) : base(rw, c)
        {
            this.nav = V_NAV;
        }

        public T exec<T>(string name, Object args)
        {
            var targs = this.nav(this, name, args);
            return this.exec<T>(targs);
        }
        public Bys exec(string name, Object args)
        {
            var targs = this.nav(this, name, args);
            return this.exec(targs);
        }
        public string exec_s(string name, Object args)
        {
            var targs = this.nav(this, name, args);
            return this.exec_s(targs);
        }

        public IDictionary<string,Object> exec_m(string name,Object args)
        {
            var targs = this.nav(this, name, args);
            return this.exec_m(targs);
        }
    }
}

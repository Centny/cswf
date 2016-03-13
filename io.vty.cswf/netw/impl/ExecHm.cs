using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;
using io.vty.cswf.util;
using System.Text.RegularExpressions;
using io.vty.cswf.log;

namespace io.vty.cswf.netw.impl
{
    public class ExecHm : r.CmdListener
    {
        public static void V_VNA(NetwRunnable nr, ExecHm rc, Bys m, out string name, out IDictionary<string, Object> args)
        {
            var res = new Dict(m.V<Dictionary<string, object>>());
            name = res.Val("name", "");
            var obj = res["args"];
            args = res["args"] as IDictionary<string,object>;
        }
        private static readonly ILog L = Log.New();
        public delegate void VNA_F(NetwRunnable nr, ExecHm rc, Bys m, out string name, out IDictionary<string, Object> args);
        public delegate Object RC_FH(RCM_Cmd rc, out bool next);
        public delegate Object RC_HH(RCM_Cmd rc);
        protected VNA_F vna;
        protected IDictionary<Regex, RC_FH> filters = new Dictionary<Regex, RC_FH>();
        protected IDictionary<Regex, RC_HH> handler = new Dictionary<Regex, RC_HH>();
        public ExecHm(VNA_F vna)
        {
            this.vna = vna;

        }
        public ExecHm()
        {
            this.vna = V_VNA;

        }

        public void onCmd(NetwRunnable nr, Bys m)
        {
            try {
                string name;
                IDictionary<string, Object> args;
                this.vna(nr, this, m, out name, out args);
                var rc = new RCM_Cmd(m, name, args);
                bool next = false;
                foreach (var filter in this.filters)
                {
                    if (!filter.Key.IsMatch(name))
                    {
                        continue;
                    }
                    var res = filter.Value(rc, out next);
                    if (next)
                    {
                        continue;
                    }
                    else
                    {
                        m.writev(res);
                        return;
                    }
                }
                foreach (var handler in this.handler)
                {
                    if (handler.Key.IsMatch(name))
                    {
                        m.writev(handler.Value(rc));
                        return;
                    }
                }
                m.writev(Util.dict("err", "function not found by name:" + name));
            }catch(Exception e)
            {
                L.E(e, "ExecHm exec error:{0}", e.Message);
                m.writev(Util.dict("err", "exec fail with " + e.Message));
            }
        }
        public void addF(string name,RC_FH f)
        {
            this.filters.Add(new Regex(name), f);
        }
        public void addH(string name, RC_HH h)
        {
            this.handler.Add(new Regex(name), h);
        }
    }
}

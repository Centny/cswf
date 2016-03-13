using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;
using io.vty.cswf.log;

namespace io.vty.cswf.netw.impl
{
    public class ExecH : r.CmdListener
    {
        private static readonly ILog L = Log.New();
        public r.CmdListener H;
        public ExecH(r.CmdListener h)
        {
            this.H = h;
        }
        public void onCmd(NetwRunnable nr, Bys m)
        {
            if (m.length < 3)
            {
                L.W("RC_S receive the command data less 3:" + m.toBs());
                return;
            }
            var pref = m.slice(0, 3);
            m.forward(3);
            this.H.onCmd(nr, m.newM(new PrefC(m, pref), m.bys, m.offset, m.length));
        }
    }
}

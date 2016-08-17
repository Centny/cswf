using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.netw.rc
{
    public interface EvnListener : r.EvnListener
    {
        void onLogin(RCRunner_m nr, String token);
    }
}

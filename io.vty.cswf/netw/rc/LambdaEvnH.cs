using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.netw.rc
{
    public class LambdaEvnH :impl.LambdaEvnH,EvnListener
    {
        public delegate void OnLoginH(RCRunner_m nr, String token);
        public OnLoginH OnLogin;

        public void onLogin(RCRunner_m nr, string token)
        {
            if (this.OnLogin != null)
            {
                this.OnLogin(nr, token);
            }
        }
    }
}

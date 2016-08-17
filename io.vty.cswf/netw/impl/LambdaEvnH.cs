using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    public class LambdaEvnH : r.EvnListener
    {
        public delegate void BegConH(NetwRunnable nr);
        public delegate void EndConH(NetwRunnable nr);
        public delegate void OnConH(NetwRunnable nr, Netw w);
        public delegate void OnErrH(NetwRunnable nr, Exception e);

        public BegConH BegCon;
        public EndConH EndCon;
        public OnConH OnCon;
        public OnErrH OnErr;

        public void begCon(NetwRunnable nr)
        {
            if (this.BegCon != null)
            {
                this.BegCon(nr);
            }
        }

        public void endCon(NetwRunnable nr)
        {
            if (this.EndCon != null)
            {
                this.EndCon(nr);
            }
        }

        public void onCon(NetwRunnable nr, Netw w)
        {
            if (this.OnCon != null)
            {
                this.OnCon(nr, w);
            }
        }

        public void onErr(NetwRunnable nr, Exception e)
        {
            if (this.OnErr != null)
            {
                this.OnErr(nr, e);
            }
        }
    }
}

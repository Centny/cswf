using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    public class QueueEvnH : r.EvnListener
    {
        public IList<r.EvnListener> HS { get; protected set; }
        public QueueEvnH(params r.EvnListener[] hs)
        {
            this.HS = new List<r.EvnListener>();
            foreach (var h in hs)
            {
                this.HS.Add(h);
            }
        }
        public QueueEvnH(IList<r.EvnListener> hs)
        {
            this.HS = new List<r.EvnListener>();
        }
        public void begCon(NetwRunnable nr)
        {
            foreach (var h in this.HS)
            {
                h.begCon(nr);
            }
        }

        public void endCon(NetwRunnable nr)
        {
            foreach (var h in this.HS)
            {
                h.endCon(nr);
            }
        }

        public void onCon(NetwRunnable nr, Netw w)
        {
            foreach (var h in this.HS)
            {
                h.onCon(nr, w);
            }
        }

        public void onErr(NetwRunnable nr, Exception e)
        {
            foreach (var h in this.HS)
            {
                h.onErr(nr, e);
            }
        }
    }
}

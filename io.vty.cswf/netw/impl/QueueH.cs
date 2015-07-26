using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers queue CmdListener.
    /// </summary>
    public class QueueH : r.CmdListener
    {
        private readonly IList<CmdListener> qs = new List<CmdListener>();

        public virtual void onCmd(NetwRunnable nr, Bys m)
        {
            foreach (CmdListener ml in this.qs)
            {
                ml.onCmd(nr, m);
            }
        }

        /// <summary>
        /// add handler to queue.
        /// </summary>
        /// <param name="l"></param>
        public virtual void addh(CmdListener l)
        {
            this.qs.Add(l);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers task mode command listener, it impl by async/await.
    /// </summary>
    public class TaskH : r.CmdListener
    {
        /// <summary>
        /// the base commond listner.
        /// </summary>
        protected virtual CmdListener h { get; set; }

        /// <summary>
        /// the constrcutor by command listener handler.
        /// </summary>
        /// <param name="h">handler</param>
        public TaskH(CmdListener h)
        {
            this.h = h;
        }

        public virtual void onCmd(NetwRunnable nr, Bys m)
        {
            new Task(i => h.onCmd(nr, m), 0).Start();
        }
    }
}

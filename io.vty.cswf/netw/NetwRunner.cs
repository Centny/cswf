using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    public abstract class NetwRunner : r.NetwRunner
    {
        /// <summary>
        /// the target stream for running.
        /// </summary>
        protected Netw rw { get; set; }

        public override Netw netw
        {
            get
            {
                return this.rw;
            }
        }

        public NetwRunner(CmdListener msg, EvnListener evn) : base(msg, evn)
        {
        }
    }
}

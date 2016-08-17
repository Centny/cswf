using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.impl;
using io.vty.cswf.util;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.dtm
{
    public class DTM_C_j : DTM_C
    {
        public NetwRunnerV.NetwBaseBuilder builder;
        public DTM_C_j(string name, FCfg cfg, rc.EvnListener evn = null) : base(name, cfg, evn)
        {
            this.builder = () =>
            {
                throw new NotImplementedException("NetwBaseBuilder is not implemented");
            };
        }
        public DTM_C_j(string name, FCfg cfg, NetwRunnerV.NetwBaseBuilder builder, rc.EvnListener evn = null) : base(name, cfg, evn)
        {
            this.builder = builder;
        }

        public override NetwRunnerV createRunner(OBDH h, r.EvnListener evn)
        {
            NetwRunnerV runner = new Wrapper(this, h, evn);
            new Task(con => runner.runc(con as Netw), runner.doCon(), TaskCreationOptions.LongRunning).Start();
            return runner;
        }
        protected class Wrapper : NetwRunnerV_j
        {
            public DTM_C_j Runner;
            public Wrapper(DTM_C_j runner, CmdListener msg, EvnListener evn) : base(msg, evn)
            {
                this.Runner = runner;
            }
            protected override NetwBase createNetwBase()
            {
                return this.Runner.createNetwBase();
            }
        }
        protected virtual NetwBase createNetwBase()
        {
            return this.builder();
        }
    }
}

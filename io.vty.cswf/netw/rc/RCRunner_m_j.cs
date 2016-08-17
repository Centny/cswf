using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;
using io.vty.cswf.netw.impl;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.rc
{
    public class RCRunner_m_j : RCRunner_m
    {
        public NetwRunnerV.NetwBaseBuilder builder;
        public RCRunner_m_j(string name, EvnListener evn = null) : base(name, evn)
        {
            this.builder = () =>
            {
                throw new NotImplementedException("NetwBaseBuilder is not implemented");
            };
        }
        public RCRunner_m_j(string name, NetwRunnerV.NetwBaseBuilder builder, EvnListener evn = null) : base(name, evn)
        {
            this.builder = builder;
        }

        public override NetwRunnerV createRunner(OBDH h, r.EvnListener evh)
        {
            NetwRunnerV runner = new Wrapper(this, h, evh);
            new Task(con => runner.runc(con as Netw), runner.doCon()).Start();
            return runner;
        }
        protected class Wrapper : NetwRunnerV_j
        {
            public RCRunner_m_j Runner;
            public Wrapper(RCRunner_m_j runner, CmdListener msg, r.EvnListener evn) : base(msg, evn)
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

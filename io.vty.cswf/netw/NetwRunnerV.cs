using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers impl createNetw and wrapper to Converter.
    /// </summary>
    public abstract class NetwRunnerV : NetwRunner, r.Converter
    {
        /// <summary>
        /// the wrapper to call runer Converter impl.
        /// </summary>
        private class Wrapper : NetwImplV
        {
            protected NetwRunnerV runner { get; set; }
            public Wrapper(NetwRunnerV runner, NetwBase rwb) : base(rwb)
            {
                this.runner = runner;
            }
            public override T B2V<T>(Bys bys)
            {
                return this.runner.B2V<T>(bys);
            }

            public override Bys V2B(Netw nv, object v)
            {
                return this.runner.V2B(nv, v);
            }
        }
        public NetwRunnerV(CmdListener msg, EvnListener evn) : base(msg, evn)
        {
        }
        protected override Netw createNetw()
        {
            this.rw = new Wrapper(this, this.createNetwBase());
            return this.rw;
        }

        protected abstract NetwBase createNetwBase();

        public abstract T B2V<T>(Bys bys);

        public abstract Bys V2B(Netw nv, object v);
    }
}

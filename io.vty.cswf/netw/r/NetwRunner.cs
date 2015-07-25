using System;
using log4net;
using System.Threading;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Provides base impl to runner interface.
    /// </summary>
    public abstract class NetwRunner : NetwRunnable
    {
        private static readonly ILog L = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract Netw netw { get; }

        /// <summary>
        /// commnad listner.
        /// </summary>
        protected virtual CmdListener msgl { get; set; }
        /// <summary>
        /// event listener.
        /// </summary>
        protected virtual EvnListener evnl { get; set; }
        /// <summary>
        /// get current running state.
        /// </summary>
        public virtual bool running { get; set; }

        public NetwRunner(CmdListener msg, EvnListener evn)
        {
            this.msgl = msg;
            this.evnl = evn;
        }

        public virtual void run_c()
        {
            L.Debug("starting running Netw");
            this.evnl.begCon(this);
            Netw nw = this.createNetw();
            this.evnl.onCon(this, nw);
            this.running = true;
            while (this.running)
            {
                try
                {
                    Bys cmd = nw.readM();
                    this.msgl.onCmd(this, cmd);
                }
                catch (EOFException e)
                {
                    this.evnl.onErr(this, e);
                    this.running = false;
                }
                catch (Exception e)
                {
                    this.evnl.onErr(this, e);
                }
            }
            this.running = false;
            L.Debug("Netw stopped");
        }

        /// <summary>
        /// waiting runner stop.
        /// </summary>
        public virtual void wcon()
        {
            while (true)
            {
                if (this.running)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }
        /// <summary>
        /// create Netw stream, it will be running on current runner.
        /// </summary>
        /// <returns>Netw stream</returns>
        protected abstract Netw createNetw();
    }
}

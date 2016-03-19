using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.r;
using io.vty.cswf.log;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.impl
{
    public abstract class RCRunnerV : r.EvnListener
    {
        private static readonly ILog L = Log.New();
        //public delegate void DailF(RCRunnerV rc,  r.EvnListener l, out NetwRunnerV runner,out ExecCm exec);
        //public delegate NetwBase DailF(RCRunnerV rc);
        //
        //public DailF Dail;
        public int Delay = 8000;
        public string Name;
        public bool Running;
        protected AutoResetEvent lck = new AutoResetEvent(false);
        protected AutoResetEvent r_lck = new AutoResetEvent(false);//runner lock
        public int Connected
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }
        protected ExecCm Exec_;
        public NetwRunnerV Runner
        {
            get;
            protected set;
        }

        public RCRunnerV(string name)
        {
            this.Name = name;
        }
        public virtual void Start()
        {
            this.Running = true;
            new Task(i => this.Try(), 0).Start();
        }
        public virtual void Stop()
        {
            this.Running = false;
            this.Runner.netw.stream.Close();
            this.Timeout();
            L.D("RC({0}) Runner is stopping", this.Name);
        }
        public virtual void Wait()
        {
            this.r_lck.WaitOne();
        }
        public virtual void begCon(NetwRunnable nr)
        {
        }

        public virtual void endCon(NetwRunnable nr)
        {
            this.Connected = 0;
            this.Runner = null;
            if (this.Running)
            {
                L.W("RC({0} connction is closed, Runner will retry connect to server", this.Name);
                new Task(i => this.Try(), 0).Start();
            }
            else
            {
                L.W("RC({0} connction is closed, Runner will stop", this.Name);
                this.r_lck.Set();
            }
        }

        public virtual void onCon(NetwRunnable nr, Netw w)
        {
            L.D("RC({0}) connect to server success", this.Name);
        }

        public virtual void onErr(NetwRunnable nr, Exception e)
        {
            L.E(e, "RC({0}) recieve error message:{1}", this.Name, e.Message);
        }

        public virtual void Try()
        {
            long last = 0;
            long now = 0;
            while (this.Running)
            {
                now = util.Util.Now();
                if (now - last < this.Delay)
                {
                    L.D("RC({0}) will retry connect to server after {1} ms", this.Name, this.Delay);
                    Thread.Sleep(this.Delay);
                }
                last = now;
                try
                {
                    this.DailRunner();
                    return;
                }
                catch (Exception e)
                {
                    L.E("RC({0}) dail to server fail with {1}", this.Name, e.Message);
                }
            }
        }

        protected abstract void DailRunner();

        public virtual void Valid()
        {
            if (this.Connected > 0)
            {
                return;
            }
            this.lck.WaitOne();
            if (this.Connected < 1)
            {
                throw new Exception("time out");
            }
        }

        public virtual void Timeout()
        {
            this.lck.Set();
        }

        public virtual T vexec<T>(string name, Object args)
        {
            this.Valid();
            return this.Exec_.exec<T>(name, args);
        }
        public virtual util.Dict vexec_m(string name, Object args)
        {
            this.Valid();
            return new util.Dict(this.Exec_.exec_m(name, args));
        }
        public virtual string vexec_s(string name, Object args)
        {
            this.Valid();
            return this.Exec_.exec_s(name, args);
        }

    }
}

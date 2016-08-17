using io.vty.cswf.log;
using io.vty.cswf.netw.impl;
using io.vty.cswf.netw.r;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.rc
{
    public abstract class RCRunner_m : RCRunnerV
    {
        private static readonly ILog L = Log.New();
        public static readonly byte CMD_S = 10;
        public static readonly byte MSG_S = 11;
        public static readonly byte CMD_C = 20;
        public static readonly byte MSG_C = 21;
        //
        public virtual QueueEvnH evnq { get; set; }
        public virtual EvnListener evnh { get; protected set; }
        public string Token;
        protected OBDH obdh;
        protected ExecHm HM;
        public NetwImplV MsgC
        {
            get;
            private set;
        }
        protected CmdListener H;
        public RCRunner_m(string name, EvnListener evn = null) : base(name)
        {
            this.HM = new ExecHm();
            this.evnq = new QueueEvnH();
            this.evnq.HS.Add(this);
            if (evn != null)
            {
                this.evnq.HS.Add(evn);
                this.evnh = evn;
            }
        }

        protected override void DailRunner()
        {
            this.Connected = 0;
            this.obdh = new OBDH();
            this.Runner = this.createRunner(this.obdh, this.evnq);
            this.Exec_ = new ExecCm(new NetwImplV.Wrapper(this.Runner, new OBDC(this.Runner.netw, CMD_S)), this.Runner);
            obdh.addh(CMD_S, this.Exec_);
            //
            obdh.addh(CMD_C, new ExecH(this.HM));
            //
            obdh.addh(MSG_C, this.H);
            //
            this.MsgC = new NetwImplV.Wrapper(this.Runner, new OBDC(this.Runner.netw, MSG_S));
            //
            this.Connected = 1;
            this.lck.Set();
            this.CallLogin();
        }

        public virtual void CallLogin()
        {
            if (String.IsNullOrWhiteSpace(this.Token))
            {
                return;
            }
            new Task(i =>
            {
                try
                {

                    this.Login(this.Token);
                }
                catch (Exception e)
                {
                    L.E(e, "RC({0}) Runner login by token({1}) faile with error({1})", this.Name, this.Token, e.Message);
                }
            }, 0).Start();
        }
        public virtual void Login(string token)
        {
            L.D("RC({0}) Runner login by token({1})", this.Name, this.Token);
            var res = this.vexec_m("login_", Util.dict("token", token));
            if (res.Val("code", -1) == 0)
            {
                L.D("RC({0}) Runner login by token({1}) success", this.Name, this.Token);
                if (this.evnh != null)
                {
                    this.evnh.onLogin(this, token);
                }
            }
            else
            {
                L.E("RC({0}) Runner login by token({1}) faile with error({1})", this.Name, this.Token, res.Val("err", ""));
                this.Stop();
                throw new Exception(res.Val("err", ""));
            }
        }

        public void addF(string name, ExecHm.RC_FH f)
        {
            this.HM.addF(name, f);
        }
        public void addH(string name, ExecHm.RC_HH h)
        {
            this.HM.addH(name, h);
        }
        public abstract NetwRunnerV createRunner(OBDH h, r.EvnListener evh);
    }
}

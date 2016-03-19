using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.impl;
using io.vty.cswf.log;
using io.vty.cswf.util;
using System.Diagnostics;
using io.vty.cswf.netw.http;
using System.Web;

namespace io.vty.cswf.netw.dtm
{
    public abstract class DTM_C : rc.RCRunner_m
    {
        public static readonly byte CMD_M_PROC = 10;
        public static readonly byte CMD_M_DONE = 20;
        private static readonly ILog L = Log.New();

        public FCfg Cfg;
        public Server Srv;
        public IDictionary<String, Process> Tasks
        {
            get;
            protected set;
        }
        public DTM_C(string name, FCfg cfg) : base(name)
        {
            this.Tasks = new Dictionary<String, Process>();
            this.Cfg = cfg;
            this.Srv = new Server();
            this.addH("start_task", this.StartTask);
            this.addH("wait_task", this.WaitTask);
            this.addH("stop_task", this.StopTask);
            this.Srv.AddF("^/proc(\\?.*)?", this.OnProc);
            this.Token = cfg.Val("token", "");
        }
        protected virtual void AddTask(String tid, Process proc)
        {
            this.Tasks[tid] = proc;
        }
        protected virtual void DelTask(String tid)
        {
            this.Tasks.Remove(tid);
        }
        public virtual Object StartTask(RCM_Cmd rc)
        {
            var tid = rc.Val("tid", "");
            var cmds = rc.Val("cmds", "");
            var res = Util.NewDict();
            if (String.IsNullOrWhiteSpace(tid) || String.IsNullOrWhiteSpace(cmds))
            {
                res["code"] = -1;
                res["err"] = "DTM_C the tid/cmds is requied";
                return res;
            }
            try
            {
                this.RunCmd(tid, cmds);
                res["code"] = 0;
                res["tid"] = tid;
            }
            catch (Exception e)
            {
                res["code"] = -2;
                res["err"] = String.Format("DTM_C start command fail with {0}", e.Message);
                L.E(e, "DTM_C start command(\n{0}\n) fail with error {1}", cmds, e.Message);
            }
            return res;
        }

        public virtual Object StopTask(RCM_Cmd rc)
        {
            var tid = rc.Val("tid", "");
            var res = Util.NewDict();
            if (String.IsNullOrWhiteSpace(tid))
            {
                res["code"] = -1;
                res["err"] = "DTM_C the tid is requied";
                return res;
            }
            try
            {
                Process proc = null;
                if (this.Tasks.TryGetValue(tid, out proc))
                {
                    proc.Kill();
                    res["code"] = 0;
                }
                else
                {
                    res["code"] = -3;
                    res["err"] = String.Format("DTM_C stop task fail with runner is not found by id({0})", tid);
                }
            }
            catch (Exception e)
            {
                res["code"] = -2;
                res["err"] = String.Format("DTM_C stop task fail with {0}", e.Message);
            }
            return res;
        }

        public virtual Object WaitTask(RCM_Cmd rc)
        {
            var tid = rc.Val("tid", "");
            var res = Util.NewDict();
            if (String.IsNullOrWhiteSpace(tid))
            {
                res["code"] = -1;
                res["err"] = "DTM_C the tid is requied";
                return res;
            }
            try
            {
                Process proc = null;
                if (this.Tasks.TryGetValue(tid, out proc))
                {
                    proc.WaitForExit();
                }
                res["code"] = 0;
            }
            catch (Exception e)
            {
                res["code"] = -2;
                res["err"] = String.Format("DTM_C wait task fail with {0}", e.Message);
            }
            return res;
        }

        public virtual void RunCmd(String tid, String cmds)
        {
            L.I("DTM_C running command(\n{0}\n) by tid({1})", cmds, tid);
            var cfg = this.Cfg.Clone();
            cfg["loc/proc_tid"] = tid;
            cmds = cfg.EnvReplaceV(cmds, false);
            L.I("DTM_C do command(\n{0}\n) by tid({1})", cmds, tid);
            this.DoCmd(tid, cfg, cmds);
        }
        public virtual void DoCmd(String tid, FCfg cfg, String cmds)
        {
            var beg = Util.Now();
            var args = Exec.ParseArgs(cmds);
            L.I("DTM_C calling command(\n{0}\n) by tid({1})", cmds, tid);
            StringBuilder sb = new StringBuilder();
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = args[0];
            if (args.Length > 1)
            {
                proc.StartInfo.Arguments = Exec.Join(args, 1, args.Length - 1);
            }
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WorkingDirectory = cfg.Val("proc_ws", ".");
            var envs = cfg.Val("proc_env", "");
            if (envs.Length > 0)
            {
                foreach (var key in Dict.parse(envs, ','))
                {
                    proc.StartInfo.EnvironmentVariables.Add(key.Key, key.Value.ToString());
                }
            }
            this.AddTask(tid, proc);
            proc.Start();
            proc.OutputDataReceived += (sender, e) =>
            {
                lock (sb)
                {
                    sb.Append(e.Data + "\n");
                }
            };
            proc.ErrorDataReceived += (sender, e) =>
            {
                lock (sb)
                {
                    sb.Append(e.Data + "\n");
                }
            };
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.Exited += (sender, e) =>
            {
                var rargs = Util.NewDict();
                var used = Util.Now() - beg;
                var res = sb.ToString();
                if (proc.ExitCode == 0)
                {
                    L.I("DTM_C running command(\n{0}\n) by tid({1}) success, used({2}ms)->\n{3}\n", cmds, tid, used, res);
                    rargs["code"] = this.DoCmdRes(rargs, cmds, res);
                }
                else
                {
                    L.I("DTM_C running command(\n{0}\n) by tid({1}) error with exit code({2})->\n{3}\n", cmds, tid, proc.ExitCode, res);
                    rargs["code"] = proc.ExitCode;
                    rargs["err"] = String.Format("exit code is {0}", proc.ExitCode);
                }
                rargs["used"] = used;
                rargs["tid"] = tid;
                this.DelTask(tid);
                this.SendDone(rargs);

            };
            proc.EnableRaisingEvents = true;
        }
        protected virtual int DoCmdRes(IDictionary<string, Object> args, String cmds, String res)
        {
            var res_a = res.Split(new string[] { "----------------result----------------" }, 2, StringSplitOptions.None);
            if (res_a.Length < 2)
            {
                return 0;
            }
            var res_l = Dict.parse('[', ']', res_a[1]);
            var json = res_l.Val("json", "");
            if (json.Length < 1)
            {
                args["data"] = res_l.data;
                return 0;
            }
            try
            {
                args["data"] = Json.parse<Dictionary<string, object>>(json);
                return 0;
            }
            catch (Exception e)
            {
                L.E(e, "DTM_C pase json result on command(\n{0}\n) by data({1}) error->{3}", cmds, json, e.Message);
                args["data"] = res_l.data;
                args["err"] = e.Message;
                return -2;
            }
        }
        protected virtual void SendDone(Object args)
        {
            this.MsgC.writev(new BysImpl(null, new byte[] { CMD_M_DONE }), args);
        }

        public virtual void NotifyProc(String tid, float rate)
        {
            var args = Util.NewDict();
            args["tid"] = tid;
            args["rate"] = rate;
            this.MsgC.writev(new BysImpl(null, new byte[] { CMD_M_PROC }), args);
        }

        public virtual HResult OnProc(Request r)
        {
            var args = HttpUtility.ParseQueryString(r.req.Url.Query);
            var tid = args.Get("tid");
            var key = this.Cfg.Val("proc_key", "progess");
            var rate_ = args.Get(key);
            if (String.IsNullOrWhiteSpace(tid) || String.IsNullOrWhiteSpace(rate_))
            {
                r.res.StatusCode = 400;
                r.WriteLine("the tid/{0} is required", key);
                return HResult.HRES_RETURN;
            }
            float rate = 0;
            if (float.TryParse(rate_, out rate))
            {
                this.NotifyProc(tid, rate);
                r.res.StatusCode = 200;
                r.WriteLine("OK");
            }
            else
            {
                r.res.StatusCode = 400;
                r.WriteLine("the {0} must be float", key);
            }
            return HResult.HRES_RETURN;
        }

        public virtual void StartProcSrv()
        {
            var prefixes_ = this.Cfg.Val("proc_prefixes", "");
            if (prefixes_.Length < 1)
            {
                L.E("DTM_C run process handler faile with prefixes is empty");
                return;
            }
            L.I("DTM_C start process server by prefixes({0})", prefixes_);
            var prefixes = prefixes_.Split(new char[] { ',' });
            foreach (var prefix in prefixes)
            {
                this.Srv.AddPrefix(prefix);
            }
            this.Srv.Start();
        }

        public virtual void StopProcSrv()
        {
            this.Srv.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.vty.cswf.netw.impl;
using io.vty.cswf.log;
using io.vty.cswf.util;
using System.Diagnostics;

namespace io.vty.cswf.netw.dtm
{
    public abstract class DTM_C : rc.RCRunner_m
    {
        public static readonly byte CMD_M_PROC = 10;
        public static readonly byte CMD_M_DONE = 20;
        private static readonly ILog L = Log.New();

        public FCfg Cfg;
        public IDictionary<String, Process> Tasks
        {
            get;
            protected set;
        }
        public DTM_C(string name, FCfg cfg) : base(name)
        {
            this.Tasks = new Dictionary<String, Process>();
            this.Cfg = cfg;
            this.addH("start_task", this.StartTask);
            this.addH("wait_task", this.WaitTask);
            this.addH("stop_task", this.StopTask);
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
            var beg = Util.Now();
            L.I("DTM_C running command(\n{0}\n) by tid({1})", cmds, tid);
            var cfg = this.Cfg.Clone();
            cfg["loc/proc_tid"] = tid;
            cmds = cfg.EnvReplaceV(cmds, false);
            L.I("DTM_C calling command(\n{0}\n) by tid({1})", cmds, tid);
            var args = Exec.ParseArgs(cmds);
            StringBuilder sb = new StringBuilder();
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = args[0];
            if (args.Length > 1)
            {
                proc.StartInfo.Arguments = string.Join(" ", args, 1, args.Length - 1);
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
    }
}

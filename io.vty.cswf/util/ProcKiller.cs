using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace io.vty.cswf.util
{
    public class ProcKiller : IDisposable
    {
        public delegate void CloseProcH(Process proc);
        public delegate void HavingNotKillH(int count);

        public static ProcKiller Shared = new ProcKiller();
        public static void AddRunning(int pid)
        {
            Shared.Running.Add(pid);
        }
        public static void DelRunning(int pid)
        {
            Shared.Running.Remove(pid);
        }

        public static void MarkUsed(int pid)
        {
            Shared.Using.Add(pid, Util.Now());
        }
        public static void MarkDone(int pid)
        {
            Shared.Using.Remove(pid);
        }

        public static void AddName(string name)
        {
            Shared.Names.Add(name);
        }
        public static void StartTimer(int period = 30000, int timeout = 60000)
        {
            Shared.Period = period;
            Shared.Timeout = timeout;
            Shared.Start();
        }
        public static void StopTimer()
        {
            Shared.Stop();
        }
        private static readonly ILog L = Log.New();
        public ICollection<int> Running { get; protected set; }
        public ICollection<int> Last { get; protected set; }
        public IDictionary<int, long> Using { get; protected set; }
        public IDictionary<int, int> Killed { get; protected set; }
        public ICollection<String> Names { get; protected set; }
        public IDictionary<int, long> NotResponsed { get; protected set; }
        public int Period { get; set; }
        public long Timeout { get; set; }
        public CloseProcH OnClose { get; set; }
        public int ShowLog { get; set; }
        public HavingNotKillH OnHavingNotKill { get; set; }
        private bool srunning;

        public ProcKiller(int period = 30000, int timeout = 60000)
        {
            this.Running = new List<int>();
            this.Last = new List<int>();
            this.Using = new Dictionary<int, long>();
            this.Killed = new Dictionary<int, int>();
            this.Names = new List<String>();
            this.NotResponsed = new Dictionary<int, long>();
            this.Period = period;
            this.Timeout = timeout;
            //this.T = new Timer(this.Clear, 0, period, period);
        }
        public void Lock()
        {
            Monitor.Enter(this);
        }
        public void Unlock()
        {
            Monitor.Exit(this);
        }
        protected virtual void Clear(object state)
        {
            try
            {
                var showlog = this.ShowLog > 0;
                Monitor.Enter(this);
                if (this.Names.Count < 1)
                {
                    return;
                }
                int found = 0, unmonitered = 0, killed = 0, monitered = 0;
                var procs = new Dictionary<int, Process>();
                foreach (var name in this.Names)
                {
                    foreach (var proc in Process.GetProcessesByName(name))
                    {
                        procs[proc.Id] = proc;
                        found += 1;
                        showlog = true;
                    }
                }
                var unknow = new HashSet<int>();
                foreach (var pid in this.Running)
                {
                    if (procs.ContainsKey(pid))
                    {
                        continue;
                    }
                    unknow.Add(pid);
                    showlog = true;
                }
                var removed = new HashSet<int>();
                foreach (var oid in this.Last)
                {
                    if (procs.ContainsKey(oid))
                    {
                        continue;
                    }
                    removed.Add(oid);
                    unmonitered += 1;
                    showlog = true;
                }
                if (this.ShowLog > 1)
                {
                    L.I("ProcKiller start do process({0}) success by found({1}),unmonitered({2}),killed({3}),monitered({4})\n"
                        + " running({5}):{6}\n"
                        + " unknow({7}):{8}\n"
                        + " using({9}):{10}\n"
                        + " notr({11}):{12}->{13}\n",
                       string.Join(",", this.Names), found, unmonitered, killed, monitered,
                       this.Running.Count, string.Join(",", this.Running),
                       unknow.Count, string.Join(",", unknow),
                       this.Using.Count, string.Join(",", this.Using),
                       this.NotResponsed.Count, string.Join(",", this.NotResponsed.Keys), string.Join(",", this.NotResponsed.Values)
                       );
                }
                var now = Util.Now();
                foreach (var proc in procs)
                {
                    if (this.Running.Contains(proc.Key))
                    {
                        if (this.Using.ContainsKey(proc.Key) && ((now - this.Using[proc.Key]) > this.Timeout))
                        {
                            this.CloseProc(proc.Value);
                            killed += 1;
                            removed.Add(proc.Key);
                            if (Killed.ContainsKey(proc.Key))
                            {
                                this.Killed[proc.Key] += 1;
                            }
                            else
                            {
                                this.Killed[proc.Key] = 1;
                            }
                        }
                        else
                        {
                            L.D("ProcKiller skipping progress({0}) for running", proc.Key);
                        }
                        continue;
                    }
                    showlog = true;
                    if (this.Last.Contains(proc.Key) ||
                        (!proc.Value.Responding && this.NotResponsed.ContainsKey(proc.Key) && now - this.NotResponsed[proc.Key] > this.Timeout))
                    {
                        this.CloseProc(proc.Value);
                        killed += 1;
                        removed.Add(proc.Key);
                        if (Killed.ContainsKey(proc.Key))
                        {
                            this.Killed[proc.Key] += 1;
                        }
                        else
                        {
                            this.Killed[proc.Key] = 1;
                        }
                        if (this.NotResponsed.ContainsKey(proc.Key))
                        {
                            this.NotResponsed.Remove(proc.Key);
                        }
                        continue;
                    }
                    if (this.NotResponsed.ContainsKey(proc.Key))
                    {
                        if (proc.Value.Responding)
                        {
                            this.NotResponsed.Remove(proc.Key);
                        }
                    }
                    else
                    {
                        if (!proc.Value.Responding)
                        {
                            L.D("ProcKiller found not responsing process({0})", proc.Key);
                            this.NotResponsed.Add(proc.Key, now);
                        }
                    }
                    this.Last.Add(proc.Key);
                    monitered += 1;
                }
                foreach (var rm in removed)
                {
                    this.Last.Remove(rm);
                    showlog = true;
                }
                //
                removed.Clear();
                var kill_time = 0;
                var not_killed = 0;
                foreach (var k in this.Killed)
                {
                    if (procs.ContainsKey(k.Key))
                    {
                        if (k.Value > kill_time)
                        {
                            kill_time = k.Value;
                        }
                        not_killed += 1;
                        continue;
                    }
                    removed.Add(k.Key);
                }
                foreach (var rm in removed)
                {
                    this.Killed.Remove(rm);
                }
                removed.Clear();
                foreach (var u in this.Using)
                {
                    if (procs.ContainsKey(u.Key))
                    {
                        continue;
                    }
                    removed.Add(u.Key);
                }
                foreach (var rm in removed)
                {
                    this.Using.Remove(rm);
                }
                if (kill_time > 3 && this.OnHavingNotKill != null)
                {
                    this.OnHavingNotKill(not_killed);
                }
                if (showlog)
                {
                    L.I("ProcKiller do clear process({0}) success by found({1}),unmonitered({2}),killed({3}),monitered({4})\n"
                        + " running({5}):{6}\n"
                        + " unknow({7}):{8}\n"
                        + " using({9}):{10}\n"
                        + " notr({11}):{12}->{13}\n",
                       string.Join(",", this.Names), found, unmonitered, killed, monitered,
                       this.Running.Count, string.Join(",", this.Running),
                       unknow.Count, string.Join(",", unknow),
                       this.Using.Count, string.Join(",", this.Using),
                       this.NotResponsed.Count, string.Join(",", this.NotResponsed.Keys), string.Join(",", this.NotResponsed.Values)
                       );
                }
            }
            catch (Exception e)
            {
                L.E(e, "ProcKill do clear process fail with error->{0}", e.Message);
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        protected virtual void loop(object state)
        {
            this.srunning = true;
            while (this.srunning)
            {
                this.Clear(state);
                Thread.Sleep(this.Period);
            }
            this.srunning = false;
        }

        public void Dispose()
        {
            this.srunning = false;
        }

        public void Start()
        {
            new Thread(this.loop).Start();
        }

        public void Stop()
        {
            this.Dispose();
        }

        protected virtual void CloseProc(Process proc)
        {
            if (this.OnClose == null)
            {
                proc.Kill();
                L.D("sending kill signal to process({0})", proc.Id);
            }
            else
            {
                this.OnClose(proc);
            }
        }
    }
}

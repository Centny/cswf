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
        public static void AddName(string name)
        {
            Shared.Names.Add(name);
        }
        public static void StartTimer(int period = 30000)
        {
            Shared.Period = period;
            Shared.Start();
        }
        public static void StopTimer()
        {
            Shared.Stop();
        }
        private static readonly ILog L = Log.New();
        public ICollection<int> Running { get; protected set; }
        public ICollection<int> Last { get; protected set; }
        public IDictionary<int, int> Killed { get; protected set; }
        public ICollection<String> Names { get; protected set; }
        public Timer T { get; protected set; }
        public int Period { get; set; }
        public CloseProcH OnClose { get; set; }
        public HavingNotKillH OnHavingNotKill { get; set; }

        public ProcKiller(int period = 30000)
        {
            this.Running = new List<int>();
            this.Last = new List<int>();
            this.Killed = new Dictionary<int, int>();
            this.Names = new List<String>();
            this.Period = period;
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
                Monitor.Enter(this);
                if (this.Names.Count < 1)
                {
                    return;
                }
                int found = 0, unmonitered = 0, killed = 0, monitered = 0;
                var procs = new Dictionary<int, Process>();
                var showlog = false;
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
                foreach (var proc in procs)
                {
                    if (this.Running.Contains(proc.Key))
                    {
                        continue;
                    }
                    if (this.Last.Contains(proc.Key))
                    {
                        this.CloseProc(proc.Value);
                        killed += 1;
                        removed.Add(proc.Key);
                        if (this.Killed.ContainsKey(proc.Key))
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
                        this.Last.Add(proc.Key);
                        monitered += 1;
                    }
                    showlog = true;
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
                if (kill_time > 3 && this.OnHavingNotKill != null)
                {
                    this.OnHavingNotKill(not_killed);
                }
                if (showlog)
                {
                    L.I("ProcKiller do clear process({5}) success by found({0}),unmonitered({1}),killed({2}),monitered({3}),running({4})\n unknow({6}):{7}\n",
                        found, unmonitered, killed, monitered, this.Running.Count, string.Join(",", this.Names), unknow.Count, string.Join(",", unknow));
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

        public void Dispose()
        {
            if (this.T == null)
            {
                return;
            }
            this.T.Dispose();
            this.T = null;
        }

        public void Start()
        {
            this.T = new Timer(this.Clear, 0, this.Period, this.Period);
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

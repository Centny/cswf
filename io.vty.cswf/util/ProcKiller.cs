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
        //public ICollection<int> Running { get; protected set; }
        //public ICollection<int> Last { get; protected set; }
        public IDictionary<int, long> Using { get; protected set; }
        public IDictionary<int, int> Killed { get; protected set; }
        public ICollection<String> Names { get; protected set; }
        public IDictionary<int, long> Running { get; protected set; }
        public int Period { get; set; }
        public long Timeout { get; set; }
        public CloseProcH OnClose { get; set; }
        public int ShowLog { get; set; }
        public HavingNotKillH OnHavingNotKill { get; set; }
        private bool srunning;

        public ProcKiller(int period = 30000, int timeout = 60000)
        {
            //this.Running = new List<int>();
            //this.Last = new List<int>();
            this.Using = new Dictionary<int, long>();
            this.Killed = new Dictionary<int, int>();
            this.Names = new List<String>();
            this.Running = new Dictionary<int, long>();
            this.Period = period;
            this.Timeout = timeout;
            //this.T = new Timer(this.Clear, 0, period, period);
        }
        public void Lock(String msg = "base")
        {
            Monitor.Enter(this);
            if (this.ShowLog > 1)
            {
                L.D("ProcKiller({0}) do lock...", msg);
            }
        }
        public void Unlock(String msg = "base")
        {
            Monitor.Exit(this);
            if (this.ShowLog > 1)
            {
                L.D("ProcKiller({0}) do unlock...", msg);
            }
        }
        protected virtual void Clear(object state)
        {
            try
            {
                this.Lock("clear");
                var showlog = this.ShowLog > 0;
                if (this.Names.Count < 1)
                {
                    return;
                }
                int found = 0, killed = 0, monitered = 0;
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
                if (this.ShowLog > 1)
                {
                    L.I("ProcKiller start do process({0}) success by found({1}),killed({2}),monitered({3})\n"
                        + " using({4}):{5}->{6}\n"
                        + " killed({7}):{8}->{9}\n"
                        + " running({10}):{11}->{12}\n",
                       string.Join(",", this.Names), found, killed, monitered,
                       this.Using.Count, string.Join(",", this.Using.Keys), string.Join(",", this.Using.Values),
                       this.Killed.Count, string.Join(",", this.Killed.Keys), string.Join(",", this.Killed.Values),
                       this.Running.Count, string.Join(",", this.Running.Keys), string.Join(",", this.Running.Values)
                       );
                }
                var now = Util.Now();
                foreach (var proc in procs)
                {
                    if (this.Using.ContainsKey(proc.Key))
                    {
                        continue;
                    }
                    if (this.Running.ContainsKey(proc.Key))
                    {
                        if (now - this.Running[proc.Key] > this.Timeout)
                        {
                            this.CloseProc(proc.Value);
                            killed += 1;
                            if (Killed.ContainsKey(proc.Key))
                            {
                                this.Killed[proc.Key] += 1;
                            }
                            else
                            {
                                this.Killed[proc.Key] = 1;
                            }
                            this.Running.Remove(proc.Key);
                            //
                            if (this.Using.ContainsKey(proc.Key))
                            {
                                this.Using.Remove(proc.Key);
                            }
                            showlog = true;
                        }
                    }
                    else
                    {
                        this.Running.Add(proc.Key, now);
                        monitered += 1;
                        showlog = true;
                    }

                }
                var kill_time = 0;
                var not_killed = 0;
                //clear killed
                {
                    var removed = new List<int>();
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
                }
                //clear using
                {
                    var removed = new List<int>();
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
                }
                //clear running
                {
                    var removed = new List<int>();
                    foreach (var u in this.Running)
                    {
                        if (procs.ContainsKey(u.Key))
                        {
                            continue;
                        }
                        removed.Add(u.Key);
                    }
                    foreach (var rm in removed)
                    {
                        this.Running.Remove(rm);
                    }
                }
                if (kill_time > 3 && this.OnHavingNotKill != null)
                {
                    this.OnHavingNotKill(not_killed);
                }
                if (showlog)
                {
                    L.I("ProcKiller start do process({0}) success by found({1}),killed({2}),monitered({3})\n"
                        + " using({4}):{5}->{6}\n"
                        + " killed({7}):{8}->{9}\n"
                        + " running({10}):{11}->{12}\n",
                       string.Join(",", this.Names), found, killed, monitered,
                       this.Using.Count, string.Join(",", this.Using.Keys), string.Join(",", this.Using.Values),
                       this.Killed.Count, string.Join(",", this.Killed.Keys), string.Join(",", this.Killed.Values),
                       this.Running.Count, string.Join(",", this.Running.Keys), string.Join(",", this.Running.Values)
                       );
                }
            }
            catch (Exception e)
            {
                L.E(e, "ProcKill do clear process fail with error->{0}", e.Message);
            }
            finally
            {
                this.Unlock("clear");
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

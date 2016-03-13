using io.vty.cswf.log;
using io.vty.cswf.netw.r;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace io.vty.cswf.netw.impl
{
    public class ExecC : CmdListener
    {
        private static readonly ILog L = Log.New();
        protected Dictionary<short, CmdListener> hs = new Dictionary<short, CmdListener>();
        protected Netw rw;
        protected short ei_cc = 1;

        public class CmdL : CmdListener
        {
            public Bys m;
            public Exception e;
            public AutoResetEvent evn = new AutoResetEvent(false);

            public void onCmd(NetwRunnable nr, Bys m)
            {
                this.m = m;
                this.evn.Set();
            }
        }
        public ExecC(Netw rw)
        {
            this.rw = rw;
        }

        public T exec<T>(byte m, Bys args)
        {
            Bys res = this.exec(m, args);
            return res.V<T>();
        }
        public Bys exec(byte m, Bys args)
        {
            CmdL cl = new CmdL();
            this.exec(m, args, cl);
            cl.evn.WaitOne();
            if (cl.e == null)
            {
                return cl.m;
            }
            else
            {
                throw cl.e;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void exec(byte m, Bys args, CmdListener l)
        {
            short nec = this.ei_cc++;
            Bys tnec = this.rw.newM(new byte[] { (byte)(nec >> 8), (byte)nec, m }, 0, 3);
            IList<Bys> ms = new List<Bys>();
            ms.Add(tnec);
            ms.Add(args);
            try
            {
                this.hs.Add(nec, l); ;
                this.rw.writeM(ms);
            }
            catch (Exception e)
            {
                this.hs.Remove(nec);
                throw e;
            }
        }
        public void onCmd(NetwRunnable nr, Bys m)
        {
            try
            {
                this.onCmd_(nr, m);
            }
            catch (Exception e)
            {
                L.W(e, "RC exec error:{1}", e.Message);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void onCmd_(NetwRunnable nr, Bys m)
        {
            if (m.length < 2)
            {
                L.W("RC receive invalid command for data less 2:" + m.toBs());
                return;
            }
            short mark = m.shortv(0);
            if (this.hs.ContainsKey(mark))
            {
                try
                {
                    m.forward(3);
                    this.hs[mark].onCmd(nr, m);
                    this.hs.Remove(mark);
                }
                catch (Exception e)
                {
                    this.hs.Remove(mark);
                    throw e;
                }
            }
            else
            {
                L.W("RC response handler not found by mark:" + mark);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void clear(Exception e)
        {
            Exception te = e;
            if (te == null)
            {
                te = new Exception("RC have been cleared");
            }
            foreach (var s in this.hs.Values)
            {
                if (!(s is CmdL))
                {
                    continue;
                }
                CmdL l = s as CmdL;
                l.e = te;
                l.evn.Set();
            }
            L.D("RC clear {0} command sucess", this.hs.Count);
            this.hs.Clear();
        }
    }
}

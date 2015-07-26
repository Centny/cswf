using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.netw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;
using io.vty.cswf.io;
using System.Runtime.Serialization;
using io.vty.cswf.netw.impl;
using io.vty.cswf.test.util;
using System.Threading;

namespace io.vty.cswf.netw.tests
{
    [TestClass()]
    public class NetwRunnerV_jTests : EvnListener
    {
        [DataContract]
        public class Obj
        {
            [DataMember(Name = "a")]
            public string A { get; set; }
            [DataMember(Name = "b")]
            public string B { get; set; }
        }

        public class Runner : NetwRunnerV_j
        {
            public NetwImplV objc { get; set; }
            public NetwImplV strc { get; set; }
            public OBDH obdh { get; set; }
            public Runner(CmdListener msg, EvnListener evn) : base(null, evn)
            {
                this.obdh = new OBDH();
                this.obdh.addh(0, new ObjH());
                this.obdh.addh(1, new StrH());
                this.msgl = this.obdh;
            }
            protected override Netw createNetw()
            {
                NetwBase nb = this.createNetwBase();
                this.rw = this.objc = new Wrapper(this, new OBDC(nb, 0));
                this.strc = new Wrapper(this, new OBDC(nb, 1));
                return this.rw;
            }
            protected override NetwBase createNetwBase()
            {
                return new NetwBaseImpl(new System.IO.BufferedStream(new PipeStream()), 102400);
            }
        }

        public class ObjH : CmdListener
        {
            public void onCmd(NetwRunnable nr, Bys m)
            {
                var msg = m.V<Obj>();
                Console.WriteLine("R->Msg(A:{0},B:{1})", msg.A, msg.B);
            }
        }

        public class StrH : CmdListener
        {
            public void onCmd(NetwRunnable nr, Bys m)
            {
                Console.WriteLine("R->Str:{0}", m.ToString());
            }
        }

        [TestMethod()]
        public void RunnerTest()
        {
            var r = new Runner(null, this);
            new Task<int>(() =>
            {
                r.run_c();
                Console.WriteLine("R->end");
                return 0;
            }, 0).Start();
            r.wcon();
            r.strc.writem(Util.bytes("abc\n"));
            Thread.Sleep(50000);
        }

        public void begCon(NetwRunnable nr)
        {
            Console.WriteLine("R->begCon");
        }

        public void onCon(NetwRunnable nr, Netw w)
        {
            Console.WriteLine("R->onCon");
        }

        public void onErr(NetwRunnable nr, Exception e)
        {
            Console.WriteLine("R->onErr:" + e);
        }
    }
}
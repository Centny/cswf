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
using io.vty.cswf.util;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace io.vty.cswf.netw.tests
{
    [TestClass()]
    public class NetwRunnerV_jTests : EvnListener
    {
        public CDL cdl;

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
            public CDL cdl { get; set; }
            public NetwImplV objc { get; set; }
            public NetwImplV strc { get; set; }
            public OBDH obdh { get; set; }
            public Runner(CmdListener msg, EvnListener evn, int c) : base(null, evn)
            {
                this.obdh = new OBDH();
                this.cdl = new CDL(c);
                this.obdh.addh(0, new ObjH(this.cdl));
                this.obdh.addh(1, new StrH(this.cdl));
                this.msgl = this.obdh;
            }
            protected override Netw createNetw()
            {
                base.createNetw();
                NetwBase nb = this.createNetwBase();
                this.rw = this.objc = new Wrapper(this, new OBDC(nb, 0));
                this.strc = new Wrapper(this, new OBDC(nb, 1));
                return this.rw;
            }
            protected override NetwBase createNetwBase()
            {
                return new NetwBaseImpl(new BytesStream(100), 102400);
            }
        }

        public class ObjH : CmdListener
        {
            public ObjH(CDL cdl)
            {
                this.cdl = cdl;
            }
            public CDL cdl { get; set; }
            public void onCmd(NetwRunnable nr, Bys m)
            {
                var msg = m.V<Obj>();
                Console.WriteLine("R->Msg(A:{0},B:{1})", msg.A, msg.B);
                this.cdl.done();
            }
        }

        public class StrH : CmdListener
        {
            public StrH(CDL cdl)
            {
                this.cdl = cdl;
            }
            public CDL cdl { get; set; }
            public void onCmd(NetwRunnable nr, Bys m)
            {
                m.stream = m.stream;
                m.limit = m.limit;
                Console.WriteLine("R->Str:{0}", m.ToString());
                this.cdl.done();
            }
        }

        [TestMethod()]
        public void RunnerTest()
        {
            this.cdl = new CDL(5);
            var r = new Runner(null, this, 21);
            new Task<int>(() =>
            {
                r.run_c();
                Console.WriteLine("R->end");
                this.cdl.done();
                return 0;
            }, 0).Start();
            r.wcon();
            for (int i = 0; i < 10; i++)
            {
                new Task<int>((idx) =>
                {
                    r.strc.writem(Util.bytes("abc-str-" + idx));
                    Obj obj = new Obj();
                    obj.A = "msg-A-" + idx;
                    obj.B = "msg-B-" + idx;
                    r.objc.writev(obj);
                    return 0;
                }, i).Start();
            }
            IList<byte[]> bys;
            //IList<Bys> cmds;
            bys= new List<Byte[]>();
            bys.Add(Util.bytes("abc-str-" + 100));
            r.strc.writem(bys);
            r.cdl.wait();
            //
            r.objc.rwb.stream.Write(new byte[5] { 1, 2, 3, 4, 5 }, 0, 5);
            r.objc.rwb.stream.Flush();
            this.cdl.wait(3);
            r.objc.rwb.stream.Close();
            this.cdl.wait();
            Console.WriteLine("done...");
        }

        public void begCon(NetwRunnable nr)
        {
            Console.WriteLine("R->begCon");
            this.cdl.done();
        }

        public void onCon(NetwRunnable nr, Netw w)
        {
            Console.WriteLine("R->onCon");
            this.cdl.done();
        }

        public void onErr(NetwRunnable nr, Exception e)
        {
            Console.WriteLine("R->onErr:" + e);
            this.cdl.done();
        }

        public void endCon(NetwRunnable nr)
        {
            Console.WriteLine("R->endCon");
            this.cdl.done();
        }
    }
}
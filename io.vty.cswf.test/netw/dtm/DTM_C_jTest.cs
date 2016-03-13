using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using io.vty.cswf.netw.dtm;
using io.vty.cswf.netw.impl;
using io.vty.cswf.io;
using io.vty.cswf.netw;
using io.vty.cswf.netw.r;
using System.Collections.Generic;
using System.Threading;
using io.vty.cswf.netw.sck;

namespace io.vty.cswf.test.netw.dtm
{
    [TestClass]
    public class DTM_C_jTest
    {

        public class DTM_C_t : DTM_C_j
        {
            public Object Done;
            public DTM_C_t(string name, FCfg cfg) : base(name, cfg)
            {

            }
            protected override void SendDone(object args)
            {
                this.Done = args;
            }
        }
        [TestMethod]
        public void TestStartDTM_C()
        {
            var cfg = new FCfg();
            cfg["proc_env"] = "a=1,b=2";
            var dtmc = new DTM_C_t("c", cfg);
            var args = Util.NewDict();
            var res = Util.NewDict();
            var tres = new Dict();
            var tid = "xx1";
            args["tid"] = tid;
            args["cmds"] = "test/dtm_json.bat abc";
            //

            var bs = new PrintStream();
            RCM_Cmd cmd;
            //
            cmd = new RCM_Cmd(null, "", args);
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            while (dtmc.Tasks.Count > 0)
            {
                Thread.Sleep(500);
            }
            Assert.AreNotEqual(null, dtmc.Done);
            res = dtmc.Done as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            //
            args["cmds"] = "test/dtm_json_err.bat abc";
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            while (dtmc.Tasks.Count > 0)
            {
                Thread.Sleep(500);
            }
            Assert.AreNotEqual(null, dtmc.Done);
            res = dtmc.Done as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            //
            //
            args["cmds"] = "sdfkfk";
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            //
            args["cmds"] = "";
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            Console.WriteLine("Done...");
        }

        [TestMethod]
        public void TestStopDTM_C()
        {
            var cfg = new FCfg();
            var dtmc = new DTM_C_t("c", cfg);
            var args = Util.NewDict();
            var res = Util.NewDict();
            var tres = new Dict();
            var tid = "xx1";
            args["tid"] = tid;
            args["cmds"] = "test/dtm_json_w.bat";
            //

            var bs = new PrintStream();
            RCM_Cmd cmd;
            cmd = new RCM_Cmd(null, "", args);
            //
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            Thread.Sleep(2000);
            Assert.AreEqual(null, dtmc.Done);
            //
            res = dtmc.StopTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", 0));
            //
            while (dtmc.Tasks.Count > 0)
            {
                Thread.Sleep(500);
            }
            Assert.AreNotEqual(null, dtmc.Done);
            res = dtmc.Done as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            //
            //
            res = dtmc.StopTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            //
            args["tid"] = "";
            res = dtmc.StopTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            Console.WriteLine("Done...");
        }

        [TestMethod]
        public void TestWaitDTM_C()
        {
            var cfg = new FCfg();
            var dtmc = new DTM_C_t("c", cfg);
            var args = Util.NewDict();
            var res = Util.NewDict();
            var tres = new Dict();
            var tid = "xx1";
            args["tid"] = tid;
            args["cmds"] = "test/dtm_json_w2.bat";
            //

            var bs = new PrintStream();
            RCM_Cmd cmd;
            cmd = new RCM_Cmd(null, "", args);
            //
            res = dtmc.StartTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            Thread.Sleep(2000);
            Assert.AreEqual(null, dtmc.Done);
            //
            res = dtmc.WaitTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            while (dtmc.Tasks.Count > 0)
            {
                Thread.Sleep(500);
            }
            Assert.AreNotEqual(null, dtmc.Done);
            //
            //
            res = dtmc.WaitTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreEqual(0, tres.Val("code", -1));
            //
            args["tid"] = "";
            res = dtmc.WaitTask(cmd) as Dictionary<String, object>;
            tres = new Dict(res);
            Console.WriteLine(Json.stringify(res));
            Assert.AreNotEqual(0, tres.Val("code", 0));
            //
            Console.WriteLine("Done...");
        }

        [TestMethod]
        public void TestDTMC()
        {
            var cfg = new FCfg();
            DTM_C_j rc = new DTM_C_j("Test", cfg, new SckDailer("127.0.0.1", 13424).Dail);
            rc.Start();
            rc.Login("abc");
            try
            {
                new DTM_C_j("Test", cfg).builder();
            }
            catch (Exception)
            {

            }
            rc.Stop();
            Thread.Sleep(2000);
        }
    }
}

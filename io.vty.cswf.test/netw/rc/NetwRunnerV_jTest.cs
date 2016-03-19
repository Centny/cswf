using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.io;
using System.Threading.Tasks;
using io.vty.cswf.netw.rc;
using io.vty.cswf.netw.r;
using System.IO;
using io.vty.cswf.netw;
using io.vty.cswf.netw.sck;
using io.vty.cswf.util;
using io.vty.cswf.netw.impl;

namespace io.vty.cswf.test.netw.rc
{
    [TestClass]
    public class NetwRunnerV_jTest
    {
        public Object c_arg(RCM_Cmd rc)
        {
            return rc.data;
        }
        [TestMethod]
        public void TestRunner()
        {
            RCRunner_m_j rc = new RCRunner_m_j("Test", new SckDailer("192.168.2.57", 2012).Dail);
            rc.addH("c_arg", cmd=>
            {
                return cmd.data;
            });
            rc.addF("c_arg", (RCM_Cmd cmd, out bool next) =>
            {
                next = true;
                return cmd.data;
            });
            rc.Start();
            rc.Login("abc");
            try {
                var args = Util.NewDict();
                args["val"] = "abc";
                var data_s = rc.vexec_s("args_s", args);
                Assert.AreEqual("abc", data_s);
                //
                args = Util.NewDict();
                args["a"] = 1;
                args["b"] = "xyz";
                var data_m = rc.vexec_m("args_m", args);
                Console.WriteLine(data_m);
                Assert.AreEqual(1, data_m.Val("a", 0));
                Assert.AreEqual("xyz", data_m.Val("b", ""));
                //
                args = Util.NewDict();
                args["name"] = "c_arg";
                var c_arg= Util.NewDict();
                c_arg["a1"] = 1;
                c_arg["a2"] = "xx";
                args["args"] = c_arg;
                var data_c = rc.vexec_m("call_c", args);
                Assert.AreEqual(1, data_c.Val("a1", 0));
                Assert.AreEqual("xx", data_c.Val("a2", ""));
                args["name"] = "xx";
                data_c = rc.vexec_m("call_c", args);
                Assert.AreEqual(true, data_c.Val("err", "").Length > 0);
            }
            catch(Exception e)
            {

                throw e;
            }
            finally
            {
                rc.Stop();
            }
        }
    }
}

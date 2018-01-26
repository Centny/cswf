using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System.IO;
using System.Net;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class FCfgTest
    {
        [TestMethod]
        public void FCfgLoadTest()
        {
            var cfg = new FCfg();
            cfg.Load("./test/fcfg_a.properties");
            Console.WriteLine(cfg);
            Assert.AreEqual("1", cfg.Val("a/a1", ""));
            Assert.AreEqual(1, cfg.Val("a/a1", 0));
            Assert.AreEqual("1", cfg.Val("b/b1", ""));
            Assert.AreEqual(1, cfg.Val("b/b1", 0));
            Assert.AreEqual(3, cfg.Secs.Count);
            //
            cfg = new FCfg();
            cfg.Load("file://" + Path.GetFullPath("test/fcfg_a.properties"));
            Assert.AreEqual("1", cfg.Val("a/a1", ""));
            Assert.AreEqual("1", cfg.Val("a/a1", ""));
            Assert.AreEqual(1, cfg.Val("a/a1", 0));
            Assert.AreEqual("1", cfg.Val("b/b1", ""));
            Assert.AreEqual(1, cfg.Val("b/b1", 0));
            Assert.AreEqual(3, cfg.Secs.Count);
        }

        [TestMethod]
        public void FCfgLoad2Test()
        {
            var cfg = new FCfg();
            cfg.Load("./test/fcfg_data.properties");
            Console.WriteLine(cfg);
            Assert.AreEqual("b/", cfg.Val("wxk", ""));
            
        }

        [TestMethod]
        public void FCfgLoadRemoteTest()
        {
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var cfg = new FCfg();
            cfg.Load("https://test:123@wxx.loc/test.properties");
            Console.WriteLine(cfg);
            Assert.AreEqual("1", cfg.Val("xa", ""));

        }
    }
}

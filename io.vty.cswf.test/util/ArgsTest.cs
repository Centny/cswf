using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;

namespace io.vty.cswf.test.util
{
    /// <summary>
    /// Summary description for ArgsTest
    /// </summary>
    [TestClass]
    public class ArgsTest
    {

        [TestMethod]
        public void TestParse()
        {
            Args args;
            //
            args = Args.parseArgs(new string[] { "a" });
            Assert.AreEqual(1, args.Vals.Count);
            //
            args = Args.parseArgs(new string[] { "a", "b" });
            Assert.AreEqual(2, args.Vals.Count);
            //
            args = Args.parseArgs(new string[] { "a", "b", "c" });
            Assert.AreEqual(3, args.Vals.Count);
            //
            args = Args.parseArgs(new string[] { "a", "b", "", "c" });
            Assert.AreEqual(3, args.Vals.Count);
            //
            args = Args.parseArgs(new string[] { "-a" });
            Assert.AreEqual(0, args.Vals.Count);
            Assert.AreEqual(1, args.Kvs.Count);
            //
            args = Args.parseArgs(new string[] { "-a", "b" });
            Assert.AreEqual(0, args.Vals.Count);
            Assert.AreEqual(1, args.Kvs.Count);
            //
            args = Args.parseArgs(new string[] { "-a", "b", "c" });
            Assert.AreEqual(1, args.Vals.Count);
            Assert.AreEqual(1, args.Kvs.Count);
            //
            args = Args.parseArgs(new string[] { "a" }, new string[] { "-a", "-b", "c" });
            Assert.AreEqual(0, args.Vals.Count);
            Assert.AreEqual(2, args.Kvs.Count);
            //
            args = Args.parseArgs(new string[] { "a" }, new string[] { "-a", "xx", "-b", "c" });
            Assert.AreEqual(1, args.Vals.Count);
            Assert.AreEqual(2, args.Kvs.Count);

            //
            //
            //
            int iv = 0;
            float fv = 0;
            string sv = "";
            args = Args.parseArgs(new string[] { "e" }, new string[] { "-i", "111", "-f", "11.2", "-s", "xxx", "-e" });
            Assert.AreEqual(0, args.Vals.Count);
            Assert.AreEqual(4, args.Kvs.Count);
            Assert.AreEqual(true, args.IntVal("i", out iv));
            Assert.AreEqual(false, args.IntVal("f", out iv));
            Assert.AreEqual(false, args.IntVal("xx", out iv));
            Assert.AreEqual(true, args.FloatVal("f", out fv));
            Assert.AreEqual(false, args.FloatVal("xx", out fv));
            Assert.AreEqual(true, args.StringVal("s", out sv));
            Assert.AreEqual("xxx", sv);
            Assert.AreEqual(false, args.StringVal("xx", out sv));
            Assert.AreEqual(true, args.Exist("e"));
        }
    }
}

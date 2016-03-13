using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class ExecTests
    {
        [TestMethod]
        public void TestExec()
        {
            //
            string data;
            var res1 = Exec.exec(out data, "..\\..\\exec1.bat");
            Assert.AreEqual(0, res1);
            Assert.AreEqual("ss", data.Trim());
            Console.WriteLine(data);
            //
            var res2 = Exec.exec(out data, "..\\..\\exec2.bat", "a", "b");
            Assert.AreEqual(0, res2);
            Assert.AreEqual("a b", data.Trim());
            Console.WriteLine(data);
        }
        [TestMethod]
        public void TestExec2()
        {
            //
            string data;
            var res1 = Exec.exec(out data, "..\\..\\exec1.bat");
            Assert.AreEqual(0, res1);
            Assert.AreEqual("ss", data.Trim());
            Console.WriteLine(data);
            //
            var res2 = Exec.exec(out data, "..\\..\\exec2.bat a b");
            Assert.AreEqual(0, res2);
            Assert.AreEqual("a b", data.Trim());
            Console.WriteLine(data);
        }

        [TestMethod]
        public void TestArgs()
        {
            var args = Exec.ParseArgs("a a b");
            Assert.AreEqual(3, args.Length);
            args = Exec.ParseArgs("'a a' b");
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("a a", args[0]);
            args = Exec.ParseArgs("\"a a\" b");
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("a a", args[0]);
        }
    }
}

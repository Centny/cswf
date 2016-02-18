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
            var res1 = Exec.exec("..\\..\\exec1.bat");
            Assert.AreEqual("ss", res1.Trim());
            Console.WriteLine(res1);
            //
            var res2 = Exec.exec("..\\..\\exec2.bat", "a", "b");
            Assert.AreEqual("a b", res2.Trim());
            Console.WriteLine(res2);
        }
    }
}

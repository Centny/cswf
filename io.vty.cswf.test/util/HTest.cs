using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System.Collections.Generic;
using System.Net;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class HTest
    {
        [TestMethod]
        public void TesDoGet()
        {
            var res = H.doGet("http://127.0.0.1:8090/g_args?a={0}&b={1}&c={2}", 1, 2, "xx");
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Console.WriteLine(res.Data);
            var data = res.toDict();
            Assert.AreEqual("1", data["a"].ToString());
            Assert.AreEqual("2", data["b"].ToString());
            Assert.AreEqual("xx", data["c"].ToString());
        }
    }
}

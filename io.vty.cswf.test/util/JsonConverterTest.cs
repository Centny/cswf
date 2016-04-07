using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;
using io.vty.cswf.util;

namespace io.vty.cswf.test.util
{
    public class TestA
    {
        [M2S(Name = "a", Emit = true)]
        public string A { get; set; }
        [M2S(Name = "b", Emit = true)]
        public string B { get; set; }
    }
    [TestClass]
    public class JsonConverterTest
    {
        [TestMethod]
        public void TestJson()
        {
            var js = new JavaScriptSerializer();
            js.RegisterConverters(new JavaScriptConverter[] { JsonConverter.createConverter<TestA>() });
            var data = js.Serialize(new TestA { A = "xxx" });
            Console.WriteLine(data);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace io.vty.cswf.util.tests
{
    [TestClass()]
    public class JsonTests
    {
        public class ObjA
        {
            public ObjA()
            {

            }
            public ObjA(string a, string b)
            {
                this.A = a;
                this.B = b;
            }
            [M2S(Name = "a")]
            public string A { get; set; }

            [M2S(Name = "b")]
            public string B { get; set; }
        }

        public class ObjB
        {
            public ObjB()
            {
                this.As = new List<ObjA>();
            }
            [M2S(Name = "as")]
            public IList<ObjA> As { get; set; }
        }

        [TestMethod()]
        public void JsonTest()
        {
            ObjB b = new ObjB();
            b.As.Add(new ObjA("A1", "B1"));
            b.As.Add(new ObjA("A2", "B2"));
            string json = Json.stringify(b);
            Console.WriteLine(json);
            //
            var js = new JavaScriptSerializer();
            js.RegisterConverters(new JavaScriptConverter[] { new JsonConverter(new Type[] { typeof(ObjA), typeof(ObjB) }) });
            ObjB b2 = js.Deserialize<ObjB>(json);
            //b = Json.parse<ObjB>(json);
            Assert.AreEqual(b.As[0].A, b2.As[0].A);
            Assert.AreEqual(b.As[1].A, b2.As[1].A);
        }

        [TestMethod()]
        public void DictTest()
        {
            ObjB b = new ObjB();
            b.As.Add(new ObjA("A1", "B1"));
            b.As.Add(new ObjA("A2", "B2"));
            string json = Json.stringify(b);
            Console.WriteLine(json);
            //
            var res = Json.toDict(json);
            Console.WriteLine(json);
        }

        [TestMethod()]
        public void StringTest()
        {
            Console.WriteLine(Json.stringify("xx"));
        }
    }
}
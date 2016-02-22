using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace io.vty.cswf.util.tests
{
    [TestClass()]
    public class JsonTests
    {
        [DataContract]
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
            [DataMember(Name = "a")]
            public string A { get; set; }

            [DataMember(Name = "b")]
            public string B { get; set; }
        }

        [DataContract]
        public class ObjB
        {
            public ObjB()
            {
                this.As = new List<ObjA>();
            }
            [DataMember(Name = "as")]
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
            b = Json.parse<ObjB>(json);
            Console.WriteLine(b.As[1].A);
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
    }
}
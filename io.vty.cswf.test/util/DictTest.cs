using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.vty.cswf.util;
using System.Collections.Generic;

namespace io.vty.cswf.test.util
{
    [TestClass]
    public class DictTest
    {
        public class ClsB
        {
            [M2S(Name = "a")]
            public String A { get; set; }
            [M2S(Name = "b")]
            public String B { get; set; }
        }
        public class ClsA
        {
            public ClsA()
            {
                this.OValL = new List<ClsB>();
            }
            [M2S(Name = "ival")]
            public int IVal { get; set; }
            [M2S(Name = "ival_a")]
            public int[] IValA { get; set; }
            [M2S(Name = "ival_l")]
            public List<int> IValL { get; set; }
            //
            [M2S(Name = "fval")]
            public float FVal { get; set; }
            [M2S(Name = "fval_a")]
            public float[] FValA { get; set; }
            [M2S(Name = "fval_l")]
            public List<float> FValL { get; set; }
            //
            [M2S(Name = "sval")]
            public string SVal { get; set; }
            [M2S(Name = "sval_a")]
            public string[] SValA { get; set; }
            [M2S(Name = "sval_l")]
            public List<string> SValL { get; set; }
            //
            //
            [M2S(Name = "oval")]
            public ClsB OVal { get; set; }
            [M2S(Name = "oval_a")]
            public ClsB[] OValA { get; set; }
            [M2S(Name = "oval_l")]
            public List<ClsB> OValL { get; set; }
        }
        [TestMethod]
        public void ParseTest()
        {
            var dict = new Dict();
            dict["ival"] = 1;
            dict["ival_a"] = new int[] { 1, 2, 3 };
            dict["ival_l"] = new List<int> { 10, 20, 30 };
            //
            dict["fval"] = 1.2;
            dict["fval_a"] = new float[] { 1, 2, 3 };
            dict["fval_l"] = new List<float> { 10, 20, 30 };
            //
            dict["sval"] = "val";
            dict["sval_a"] = new string[] { "a", "b", "c" };
            dict["sval_l"] = new List<string> { "a1", "b1", "c1" };
            //
            dict["oval"] = Util.dict("a", "x1");
            dict["oval_a"] = new IDictionary<string, object>[] {
                Util.dict("a", "x2"),
                Util.dict("a", "x3"),
                Util.dict("a", "x4"),
            };
            dict["oval_l"] = new List<IDictionary<string, object>> {
                Util.dict("a", "x2"),
                Util.dict("a", "x3"),
                Util.dict("a", "x4"),
            };
            var res = (ClsA)dict.Parse(typeof(ClsA));
            //
            Assert.AreEqual(1, res.IVal);
            Assert.AreEqual(3, res.IValA.Length);
            Assert.AreEqual(1, res.IValA[0]);
            Assert.AreEqual(3, res.IValL.Count);
            Assert.AreEqual(10, res.IValL[0]);
            //
            Assert.AreEqual(1.2f, res.FVal);
            Assert.AreEqual(3, res.FValA.Length);
            Assert.AreEqual(3, res.FValL.Count);
            //
            Assert.AreEqual("val", res.SVal);
            Assert.AreEqual(3, res.SValA.Length);
            Assert.AreEqual(3, res.SValL.Count);
            //
            Assert.AreEqual("x1", res.OVal.A);
            Assert.AreEqual(3, res.OValA.Length);
            Assert.AreEqual(3, res.OValL.Count);
        }
        [TestMethod]
        public void ParseTest2()
        {
            var dict = new Dict();
            dict["ival"] = 1f;
            dict["ival_a"] = new float[] { 1, 2, 3 };
            dict["ival_l"] = new List<float> { 10, 20, 30 };
            //
            dict["fval"] = 1;
            dict["fval_a"] = new int[] { 1, 2, 3 };
            dict["fval_l"] = new List<int> { 10, 20, 30 };
            //
            dict["sval"] = "val";
            dict["sval_a"] = new string[] { "a", "b", "c" };
            dict["sval_l"] = new List<string> { "a1", "b1", "c1" };
            //
            dict["oval"] = Util.dict("a", "x1");
            dict["oval_a"] = new IDictionary<string, object>[] {
                Util.dict("a", "x2"),
                Util.dict("a", "x3"),
                Util.dict("a", "x4"),
            };
            dict["oval_l"] = new List<IDictionary<string, object>> {
                Util.dict("a", "x2"),
                Util.dict("a", "x3"),
                Util.dict("a", "x4"),
            };
            var res = (ClsA)dict.Parse(typeof(ClsA));
            //
            Assert.AreEqual(1, res.IVal);
            Assert.AreEqual(3, res.IValA.Length);
            Assert.AreEqual(1, res.IValA[0]);
            Assert.AreEqual(3, res.IValL.Count);
            Assert.AreEqual(10, res.IValL[0]);
            //
            Assert.AreEqual(1f, res.FVal);
            Assert.AreEqual(3, res.FValA.Length);
            Assert.AreEqual(3, res.FValL.Count);
            //
            Assert.AreEqual("val", res.SVal);
            Assert.AreEqual(3, res.SValA.Length);
            Assert.AreEqual(3, res.SValL.Count);
            //
            Assert.AreEqual("x1", res.OVal.A);
            Assert.AreEqual(3, res.OValA.Length);
            Assert.AreEqual(3, res.OValL.Count);
        }
        [TestMethod]
        public void testDict()
        {
            var res = H.doGet("http://sso.dev.gdy.io/sso/api/login?usr=c2&pwd=123456");
            var data = res.toUtilDict();
            var innerData = data.Val<Dict>("data", null);
            Assert.AreEqual(true, innerData.Val<string>("token", "").Length > 0);
        }
    }
}

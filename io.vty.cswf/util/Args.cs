using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util
{
    public class Args
    {
        public string[] NKV;
        public IList<string> Vals;
        public IDictionary<string, string> Kvs;

        public Args()
        {
            this.NKV = new string[] { };
            this.Kvs = new Dictionary<string, string>();
            this.Vals = new List<string>();
        }

        public Args(string[] nkv)
        {
            this.NKV = nkv;
            this.Kvs = new Dictionary<string, string>();
            this.Vals = new List<string>();
        }

        public bool IntVal(string key, out int val, int defaulv = 0)
        {
            if (this.Kvs.ContainsKey(key))
            {
                return int.TryParse(this.Kvs[key], out val);
            }
            else
            {
                val = defaulv;
                return false;
            }
        }
        public bool IntVal(int idx, out int val, int defaulv = 0)
        {
            if (idx >= 0 && idx < this.Vals.Count)
            {
                return int.TryParse(this.Vals[idx], out val);
            }
            else
            {
                val = defaulv;
                return false;
            }
        }
        public bool FloatVal(string key, out float val, float defaultv = 0)
        {
            if (this.Kvs.ContainsKey(key))
            {
                return float.TryParse(this.Kvs[key], out val);
            }
            else
            {
                val = defaultv;
                return false;
            }
        }
        public bool FloatVal(int idx, out float val, float defaultv = 0)
        {
            if (idx >= 0 && idx < this.Vals.Count)
            {
                return float.TryParse(this.Vals[idx], out val);
            }
            else
            {
                val = defaultv;
                return false;
            }
        }
        public bool DoubleVal(string key, out double val, double defaultv = 0)
        {
            if (this.Kvs.ContainsKey(key))
            {
                return double.TryParse(this.Kvs[key], out val);
            }
            else
            {
                val = defaultv;
                return false;
            }
        }
        public bool DoubleVal(int idx, out double val, double defaultv = 0)
        {
            if (idx >= 0 && idx < this.Vals.Count)
            {
                return double.TryParse(this.Vals[idx], out val);
            }
            else
            {
                val = defaultv;
                return false;
            }
        }
        public bool StringVal(string key, out string val, string defaultv = "")
        {
            if (this.Kvs.ContainsKey(key))
            {
                val = this.Kvs[key];
            }
            else
            {
                val = defaultv;
            }
            return val.Length > 0;
        }

        public bool StringVal(int idx, out string val, string defaultv = "")
        {
            if (idx >= 0 && idx < this.Vals.Count)
            {
                val = this.Vals[idx];
            }
            else
            {
                val = defaultv;
            }
            return val.Length > 0;
        }

        public bool Exist(string key)
        {
            return this.Kvs.ContainsKey(key);
        }

        private bool IsNkv(string key)
        {
            foreach (var nkv in NKV)
            {
                if (nkv.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public Args parse(string[] args, int beg = 0)
        {
            string key = "";
            for (int i = beg; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.Length < 1)
                {
                    continue;
                }
                if (arg[0] == '-')
                {
                    if (key.Length > 0)
                    {
                        this.Kvs.Add(key, "");
                        key = "";
                    }
                    key = arg.Substring(1);
                    if (IsNkv(key))
                    {
                        this.Kvs.Add(key, "");
                        key = "";
                    }
                }
                else
                {
                    if (key.Length < 1)
                    {
                        this.Vals.Add(arg);
                    }
                    else
                    {
                        this.Kvs.Add(key, arg);
                        key = "";
                    }
                }
            }
            if (key.Length > 0)
            {
                this.Kvs.Add(key, "");
            }
            return this;
        }

        public void Print()
        {
            Console.WriteLine(" Kvs");
            foreach (var key in this.Kvs.Keys)
            {
                Console.WriteLine("  {0}:\t{1}", key, this.Kvs[key]);
            }
            Console.WriteLine(" NKV");
            foreach (var nkv in this.NKV)
            {
                Console.WriteLine("  {0}", nkv);
            }
        }

        public static Args parseArgs(string[] args, int beg = 0)
        {
            return new Args().parse(args, beg);
        }

        public static Args parseArgs(string[] nkv, string[] args, int beg = 0)
        {
            return new Args(nkv).parse(args, beg);
        }

        public static Args parseArgs(string[] nkv, string args, int beg = 0)
        {
            return parseArgs(nkv, Exec.ParseArgs(args), beg);
        }
        public static Args parseArgs(string args, bool isAllVals = true, int beg = 0)
        {
            var argsL = Exec.ParseArgs(args);
            if (isAllVals)
            {
                var argsR = new Args(new string[] { });
                foreach (var arg in argsL)
                {
                    argsR.Vals.Add(arg);
                }
                return argsR;
            }
            else
            {
                return parseArgs(argsL, beg);
            }
        }
    }
}

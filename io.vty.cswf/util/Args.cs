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
        public bool FloatVal(string key, out float val, int defaultv = 0)
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

        public static Args parseArgs(string[] args, int beg = 0)
        {
            return new Args().parse(args, beg);
        }

        public static Args parseArgs(string[] nkv, string[] args, int beg = 0)
        {
            return new Args(nkv).parse(args, beg);
        }
    }
}

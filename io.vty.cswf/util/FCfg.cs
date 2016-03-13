using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace io.vty.cswf.util
{
    public class FCfg : Dict
    {
        public ICollection<String> Secs = new HashSet<String>();
        Regex reg_s = new Regex("^\\[[^\\]]*\\]$");
        Regex reg_l = new Regex("^@.*$");
        Regex reg_h = new Regex("^[^:]*?://.*$");
        Regex reg_c = new Regex("#.*$");
        Regex reg_arg = new Regex("\\$\\{[^\\}]*\\}");

        public String Base;
        public void Load(Stream s, bool wait = true, String base_w = ".")
        {
            StreamReader reader = new StreamReader(s);
            String line = null;
            String sec = "loc";
            this.Secs.Add("loc");
            while ((line = reader.ReadLine()) != null)
            {
                line = reg_c.Replace(line, "");
                line = line.Trim();
                if (line.Length < 1)
                {
                    continue;
                }
                if (this.reg_s.IsMatch(line))
                {
                    line = line.Trim('[', ']').Trim();
                    this.Secs.Add(line);
                    sec = line;
                    continue;
                }
                if (this.reg_l.IsMatch(line))
                {
                    line = this.EnvReplaceV(line, true);
                    this.Exec(line, wait, base_w);
                    continue;
                }
                line = this.EnvReplaceV(line, false);
                var kvs = line.Split(new char[] { '=' }, 2);
                if (kvs.Length < 2)
                {
                    Console.WriteLine("FCfg find invalid line->{0}", line);
                    continue;
                }
                this[sec + "/" + kvs[0]] = kvs[1];
            }
        }
        public void Load(String uri, bool wait = false, String base_w = ".")
        {
            if (String.IsNullOrWhiteSpace(uri))
            {
                //Console.WriteLine("FCfg load uri fail with path is empty");
                return;
            }
            uri = uri.Trim();
            if (this.reg_h.IsMatch(uri))
            {
                this.LoadUrl(uri, wait);
            }
            else
            {
                this.LoadFile(uri, wait, base_w);
            }
        }

        public virtual void LoadFile(String file, bool wait = false, String base_w = ".")
        {
            Console.WriteLine("loading local configure->{0}", file);
            var fargs_ = file.Split(new char[] { '?' }, 2);
            if (fargs_.Length > 1)
            {
                var kvs = HttpUtility.ParseQueryString(fargs_[1]);
                foreach (var k in kvs.Keys)
                {
                    this["loc/" + k] = kvs.GetValues(k as string)[0];
                }
            }
            file = fargs_[0];
            file = base_w + "\\" + file;
            base_w = Path.GetDirectoryName(file);
            String data;
            while (true)
            {
                try
                {
                    data = Util.read(file);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("FCfg load file({0}) faile with {0}", file, e.Message);
                    if (wait)
                    {
                        continue;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(data))
            {
                return;
            }
            this.Load(new MemoryStream(Encoding.UTF8.GetBytes(data)), wait, base_w);
        }

        public virtual void LoadUrl(String uri, bool wait = false)
        {
            Console.WriteLine("loading remote configure->{0}", uri);
            String base_w;
            int last = -1;
            if ((last = uri.LastIndexOf('/')) > -1)
            {
                base_w = uri.Substring(0, last);
            }
            else
            {
                base_w = uri.Split('?')[0];
            }
            String data;
            while (true)
            {
                try
                {
                    Uri ruri = new Uri(uri);
                    var web = new WebClient();
                    data = web.DownloadString(ruri);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("FCfg load uri({0}) faile with {0}", uri, e.Message);
                    if (wait)
                    {
                        continue;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(data))
            {
                return;
            }
            this.Load(new MemoryStream(Encoding.UTF8.GetBytes(data)), wait, base_w);
        }

        public virtual void Exec(String line, bool wait = false, String base_w = ".")
        {
            line = line.TrimStart('@');
            var kvs = line.Split(new char[] { ':' }, 2);
            if (kvs.Length < 2)
            {
                Console.WriteLine(line);
                return;
            }
            if ("l".Equals(kvs[0]))
            {
                this.Load(kvs[1], wait, base_w);
                return;
            }
            var cs = kvs[0].Split(new String[] { "==" }, 2, StringSplitOptions.None);
            if (cs.Length > 1)
            {
                if (cs[0] == cs[1])
                {
                    this.Exec(kvs[1], wait, base_w);
                }
                return;
            }
            cs = kvs[0].Split(new String[] { "!=" }, 2, StringSplitOptions.None);
            if (cs.Length > 1)
            {
                if (cs[0] != cs[1])
                {
                    this.Exec(kvs[1], wait, base_w);
                }
                return;
            }
            Console.WriteLine(line);
        }

        public virtual String EnvReplaceV(String src, bool empty)
        {
            return this.reg_arg.Replace(src, m =>
             {
                 var key = m.Value.Trim('$', '{', '}');
                 var keys = key.Split(',');
                 var val = "";
                 foreach (var k in keys)
                 {
                     if (this.ContainsKey(k))
                     {
                         val = this[k].ToString();
                         break;
                     }
                     if (this.ContainsKey("loc/" + k))
                     {
                         val = this["loc/" + k].ToString();
                         break;
                     }
                     if (k == "C_PWD")
                     {
                         val = this.Base;
                         break;
                     }
                     val = Environment.GetEnvironmentVariable(k);
                     if (!String.IsNullOrWhiteSpace(val))
                     {
                         break;
                     }
                 }
                 if (!String.IsNullOrWhiteSpace(val))
                 {
                     return val;
                 }
                 if (empty)
                 {
                     return "";
                 }
                 else
                 {
                     return m.Value;
                 }
             });
        }

        public override string ToString()
        {
            ICollection<String> keys = new HashSet<String>();
            ICollection<String> locs = new HashSet<String>();
            foreach (var key in this.Keys)
            {
                if (key.StartsWith("loc/"))
                {
                    locs.Add(key);
                }
                else
                {
                    keys.Add(key);
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (var key in keys)
            {
                sb.AppendFormat("{0}={1}\n", key, this[key]);
            }
            foreach (var key in locs)
            {
                sb.AppendFormat("{0}={1}\n", key, this[key]);
            }
            return sb.ToString();
        }
        public override object this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
                if (base.ContainsKey("loc/" + key))
                {
                    return base["loc/" + key];
                }
                return null;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}

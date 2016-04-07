using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace io.vty.cswf.util
{
    public class Dict : IDictionary<string, object>
    {
        public IDictionary<string, object> data;
        public Dict(IDictionary<string, object> data)
        {
            this.data = data;
        }

        public Dict()
        {
            this.data = new Dictionary<string, object>();
        }

        public virtual object this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return this.data[key];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                this.data[key] = value;
            }
        }

        public virtual int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return this.data.IsReadOnly;
            }
        }

        public virtual ICollection<string> Keys
        {
            get
            {
                return this.data.Keys;
            }
        }

        public virtual ICollection<object> Values
        {
            get
            {
                return this.data.Values;
            }
        }

        public virtual void Add(KeyValuePair<string, object> item)
        {
            this.data.Add(item);
        }

        public virtual void Add(string key, object value)
        {
            this.data.Add(key, value);
        }

        public virtual void Clear()
        {
            this.data.Clear();
        }

        public virtual bool Contains(KeyValuePair<string, object> item)
        {
            return this.data.Contains(item);
        }

        public virtual bool ContainsKey(string key)
        {
            return this.data.ContainsKey(key);
        }

        public virtual void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public virtual bool Remove(KeyValuePair<string, object> item)
        {
            return this.data.Remove(item);
        }

        public virtual bool Remove(string key)
        {
            return this.data.Remove(key);
        }

        public virtual bool TryGetValue(string key, out object value)
        {
            return this.data.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public virtual T Val<T>(string key, T def)
        {
            var type = typeof(T);
            return (T)Val(key, type, def);
        }
        public virtual object Val(string key, Type type, object def)
        {
            return Parse(this[key], type, def);
        }
        public static object Parse(object val, Type type, object def)
        {
            if (val == null)
            {
                return def;
            }
            if (val.GetType() == type)
            {
                return val;
            }
            var sval = val.ToString();
            object res = def;
            if (type == typeof(string))
            {
                res = sval;
            }
            else if (type == typeof(int))
            {
                int iv = 0;
                if (int.TryParse(sval, out iv))
                {
                    res = iv;
                }
            }
            else if (type == typeof(uint))
            {
                uint iv = 0;
                if (uint.TryParse(sval, out iv))
                {
                    res = iv;
                }
            }
            else if (type == typeof(Int64))
            {
                Int64 iv = 0;
                if (Int64.TryParse(sval, out iv))
                {
                    res = iv;
                }
            }
            else if (type == typeof(UInt64))
            {
                UInt64 iv = 0;
                if (UInt64.TryParse(sval, out iv))
                {
                    res = iv;
                }
            }
            else if (type == typeof(float))
            {
                float fv = 0;
                if (float.TryParse(sval, out fv))
                {
                    res = fv;
                }
            }
            return res;
        }

        public static Dict parse(string data, char seq)
        {
            var args = data.Split(seq);
            var kvsl = new Dict();
            foreach (var arg in args)
            {
                var varg = arg.Trim();
                var kvs = varg.Split('=');
                if (kvs.Length > 1)
                {
                    kvsl[kvs[0]] = kvs[1];
                }
                else
                {
                    kvsl[kvs[0]] = "";
                }
            }
            return kvsl;
        }

        public static Dict parse(char pre, char sub, string data)
        {
            var r_pre = new Regex(String.Format("^\\{0}[^/\\{1}]*\\{1}", pre, sub, sub));
            var r_sub = new Regex(String.Format("^\\{0}/[^\\{1}]*\\{1}", pre, sub, sub));
            var reader = new StringReader(data);
            String line, key_pre = "", key_sub = "";
            StringBuilder sb = new StringBuilder();
            var kvsl = new Dict();
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length < 1)
                {
                    continue;
                }
                if (r_pre.IsMatch(line))
                {
                    key_pre = line.Trim(pre, sub);
                    sb.Clear();
                }
                else if (r_sub.IsMatch(line))
                {
                    key_sub = line.Trim(pre, sub, '/');
                    if (key_pre == key_sub)
                    {
                        kvsl[key_pre] = sb.ToString().Trim();
                    }
                    key_pre = "";
                    key_sub = "";
                    sb.Clear();
                }
                else
                {
                    sb.AppendLine(line);
                }
            }
            return kvsl;
        }

        private void cov_err(object obj, PropertyInfo p, string name, object data_)
        {
            throw new Exception(String.Format("can't convert {0} to {1} on property {2}", data_.GetType(), p.PropertyType, name));
        }

        private void set_val(object obj, PropertyInfo p, string name)
        {
            var data_ = this[name];
            if (data_ == null)
            {
                return;
            }
            var val = this.Val(name, p.PropertyType, null);
            if (val == null)
            {
                throw new Exception(String.Format("can't convert {0} to {1} on property {2}", data_.GetType(), p.PropertyType, name));
            }
            p.SetValue(obj, val, null);
        }
        private void set_ary_obj<T>(object obj, PropertyInfo p, string name, IEnumerable data, bool ary = false)
        {
            List<T> res = new List<T>();
            foreach (var d in data)
            {
                res.Add((T)d);
            }
            if (ary)
            {
                p.SetValue(obj, res.ToArray(), null);
            }
            else
            {
                p.SetValue(obj, res, null);
            }
        }
        private void set_ary_obj_v(object obj, PropertyInfo p, string name, IEnumerable data, Type type, bool ary = false)
        {
            var res = new List<object>();
            foreach (var d in data)
            {
                var dict = d as IDictionary<string, object>;
                if (dict == null)
                {
                    this.cov_err(obj, p, name, data);
                    return;
                }
                res.Add(new Dict(dict).Parse(type));
            }
            if (ary)
            {
                var tary = Array.CreateInstance(type, res.Count);
                for (var i = 0; i < res.Count; i++)
                {
                    tary.SetValue(res[i], i);
                }
                p.SetValue(obj, tary, null);
            }
            else
            {
                var tres = p.GetValue(obj, null) as IList;
                if (tres == null)
                {
                    throw new Exception(String.Format("the {0} property return valus is not IList or null, please init it as empty Ilist"));
                }
                foreach (var i in res)
                {
                    tres.Add(i);
                }
            }
        }
        private void set_ary_num<T>(object obj, PropertyInfo p, string name, IEnumerable data, T def, bool ary = false)
        {
            List<T> res = new List<T>();
            foreach (var d in data)
            {
                var val = (T)Parse(d, typeof(T), def);
                res.Add(val);
            }
            if (ary)
            {
                p.SetValue(obj, res.ToArray(), null);
            }
            else
            {
                p.SetValue(obj, res, null);
            }
        }

        private void set_ary(object obj, PropertyInfo p, string name, IEnumerable data, Type type, bool ary)
        {
            if (Util.IsOrignialType(type))
            {
                if (type == typeof(int))
                {
                    this.set_ary_num<int>(obj, p, name, data, 0, ary);
                }
                else if (type == typeof(Int64))
                {
                    this.set_ary_num<Int64>(obj, p, name, data, 0, ary);
                }
                else if (type == typeof(float))
                {
                    this.set_ary_num<float>(obj, p, name, data, 0, ary);
                }
                else if (type == typeof(double))
                {
                    this.set_ary_num<double>(obj, p, name, data, 0, ary);
                }
                else if (type == typeof(string))
                {
                    this.set_ary_obj<string>(obj, p, name, data, ary);
                }
                else
                {
                    this.cov_err(obj, p, name, data);
                }
                return;
            }
            else
            {
                this.set_ary_obj_v(obj, p, name, data, type, ary);
            }

        }
        private void set_ary(object obj, PropertyInfo p, string name)
        {
            var data_ = this[name];
            if (data_ == null)
            {
                return;
            }
            var data = data_ as IEnumerable;
            if (data == null)
            {
                this.cov_err(obj, p, name, data_);
            }
            var args = p.PropertyType.GetGenericArguments();
            if (args.Length > 0)
            {
                this.set_ary(obj, p, name, data, args[0], false);
            }
            else
            {
                this.set_ary(obj, p, name, data, p.PropertyType.GetElementType(), true);
            }

        }

        private void set_obj(object obj, PropertyInfo p, string name)
        {
            var data_ = this[name];
            if (data_ == null)
            {
                return;
            }
            var data = data_ as IDictionary<string, object>;
            if (data == null)
            {
                this.cov_err(obj, p, name, data_);
            }
            var dict = new Dict(data);
            p.SetValue(obj, dict.Parse(p.PropertyType), null);
        }

        public Object Parse(Type type)
        {
            var obj = Activator.CreateInstance(type);
            foreach (var p in type.GetProperties())
            {
                if (!p.CanWrite)
                {
                    continue;
                }
                var name = p.Name;
                var attr = Attribute.GetCustomAttribute(p, typeof(M2S)) as M2S;
                if (attr != null)
                {
                    if (attr.Ignore)
                    {
                        continue;
                    }
                    if (!String.IsNullOrWhiteSpace(attr.Name))
                    {
                        name = attr.Name;
                    }
                }
                if (!this.ContainsKey(name))
                {
                    continue;
                }
                if (Util.IsOrignialType(p.PropertyType))
                {
                    this.set_val(obj, p, name);
                }
                else if (Util.IsImpl(p.PropertyType, typeof(IEnumerable)))
                {
                    this.set_ary(obj, p, name);
                }
                else
                {
                    this.set_obj(obj, p, name);
                }
            }
            return obj;
        }
    }
}

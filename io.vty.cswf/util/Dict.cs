using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            object val = this[key];
            if (val == null)
            {
                return def;
            }
            var type = typeof(T);
            if (val.GetType() == type)
            {
                return (T)val;
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
            return (T)res;
        }
    }
}

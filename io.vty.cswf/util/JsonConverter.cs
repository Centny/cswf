using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace io.vty.cswf.util
{
    public class JsonConverter : JavaScriptConverter
    {
        public static JavaScriptSerializer CreateJavaScriptSerializer(Type type)
        {
            var js = new JavaScriptSerializer();
            js.RegisterConverters(new JavaScriptConverter[] { JsonConverter.createConverter(type) });
            return js;
        }
        public static JavaScriptSerializer CreateJavaScriptSerializer<T>()
        {
            return CreateJavaScriptSerializer(typeof(T));
        }
        public static JsonConverter createConverter<T>()
        {
            return new JsonConverter(ListTypes<T>());
        }
        public static JsonConverter createConverter(Type type)
        {
            return new JsonConverter(ListTypes(type));
        }
        public static JavaScriptSerializer createSerializer<T>()
        {
            var ss = new JavaScriptSerializer();
            ss.RegisterConverters(new JavaScriptConverter[] { createConverter<T>() });
            return ss;
        }
        public static JavaScriptSerializer createSerializer(Type type)
        {
            var ss = new JavaScriptSerializer();
            ss.RegisterConverters(new JavaScriptConverter[] { createConverter(type) });
            return ss;
        }
        public static List<Type> ListTypes<T>()
        {
            var types = new List<Type>();
            ListTypes(types, typeof(T));
            return types;
        }
        public static List<Type> ListTypes(Type type)
        {
            var types = new List<Type>();
            ListTypes(types, type);
            return types;
        }
        public static void ListTypes(List<Type> types, Type type)
        {
            if (Util.IsOrignialType(type))
            {
                return;
            }
            if (Util.IsImpl(type, typeof(IEnumerable)))
            {
                var args = type.GetGenericArguments();
                if (args.Length > 0)
                {
                    ListTypes(types, args[0]);
                }
                else
                {
                    ListTypes(types, type.GetElementType());
                }
                return;
            }
            if (types.Contains(type))
            {
                return;
            }
            types.Add(type);
            foreach (var p in type.GetProperties())
            {
                if (!p.CanRead)
                {
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(p, typeof(M2S)) as M2S;
                if (attr != null && attr.Ignore)
                {
                    continue;
                }
                ListTypes(types, p.PropertyType);
            }
            foreach (var m in type.GetMethods())
            {
                if (m.GetParameters().Length < 1)
                {
                    continue;
                }
                if (!m.Name.StartsWith("get_"))
                {
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(m, typeof(M2S)) as M2S;
                if (attr == null || attr.Ignore)
                {
                    continue;
                }
                ListTypes(types, m.ReturnType);
            }
        }
        private IEnumerable<Type> types;
        public JsonConverter(IEnumerable<Type> types)
        {
            this.types = types;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return this.types;
            }
        }

        public static bool IsImpl(Type type, Type impl)
        {
            foreach (var i in type.GetInterfaces())
            {

                if (i.GetGenericTypeDefinition() == impl)
                {
                    return true;
                }
            }
            return false;
        }
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return new Dict(dictionary).Parse(type);
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var type = obj.GetType();
            var res = new Dictionary<string, object>();
            foreach (var p in type.GetProperties())
            {
                if (!p.CanRead)
                {
                    continue;
                }
                var name = p.Name;
                var attr = Attribute.GetCustomAttribute(p, typeof(M2S)) as M2S;
                if (attr == null)
                {
                    res[name] = p.GetValue(obj, null);
                    continue;
                }
                if (!String.IsNullOrWhiteSpace(attr.Name))
                {
                    name = attr.Name;
                }
                if (attr.Ignore)
                {
                    continue;
                }
                if (!attr.Emit)
                {
                    res[name] = p.GetValue(obj, null);
                    continue;
                }
                var val = p.GetValue(obj, null);
                if (val == null)
                {
                    continue;
                }
                if (val is string && string.IsNullOrWhiteSpace(val as string))
                {
                    continue;
                }
                else
                {
                    res[name] = val;
                }
            }
            /*
            foreach (var m in type.GetMethods())
            {
                if (m.GetParameters().Length < 1)
                {
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(m, typeof(M2S)) as M2S;
                if (attr == null || attr.Ignore)
                {
                    continue;
                }
                res[attr.Name] = m.Invoke(obj, null);
            }
            */
            return res;
        }
    }
}

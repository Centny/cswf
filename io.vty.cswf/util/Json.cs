using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.util
{
    /// <summary>
    /// Providers util to covernt between json and object.
    /// </summary>
    public class Json
    {
        /// <summary>
        /// parse json to objec.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="json">json string</param>
        /// <returns>object instance</returns>
        public static T parse<T>(byte[] json, int off, int len)
        {
            using (var ms = new MemoryStream(json, off, len, false))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        /// <summary>
        /// parse json to objec.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="json">json string</param>
        /// <returns>object instance</returns>
        public static T parse<T>(byte[] json)
        {
            return parse<T>(json, 0, json.Length);
        }

        /// <summary>
        /// parse json to objec.
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="json">json string</param>
        /// <returns>object instance</returns>
        public static T parse<T>(string json)
        {
            return parse<T>(Encoding.Default.GetBytes(json));
        }

        /// <summary>
        /// stringify object to json.
        /// </summary>
        /// <param name="v">targe object</param>
        /// <returns>json string</returns>
        public static byte[] stringify_(object v)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(v.GetType()).WriteObject(ms, v);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// stringify object to json.
        /// </summary>
        /// <param name="v">targe object</param>
        /// <returns>json string</returns>
        public static string stringify(object v)
        {
            return Encoding.UTF8.GetString(stringify_(v));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
            return parse<T>(Encoding.UTF8.GetString(json, off, len));
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
            var js = JsonConverter.CreateJavaScriptSerializer<T>();
            return js.Deserialize<T>(json);
        }

        /// <summary>
        /// stringify object to json.
        /// </summary>
        /// <param name="v">targe object</param>
        /// <returns>json string</returns>
        public static byte[] stringify_(object v)
        {
            return Encoding.UTF8.GetBytes(stringify(v));
        }

        /// <summary>
        /// stringify object to json.
        /// </summary>
        /// <param name="v">targe object</param>
        /// <returns>json string</returns>
        public static string stringify(object v)
        {
            var type = v.GetType();
            var js = JsonConverter.CreateJavaScriptSerializer(type);
            return js.Serialize(v);
        }

        public static IDictionary<string, object> toDict(string json)
        {
            return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
        }

    }
}

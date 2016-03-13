using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.util
{
    /// <summary>
    /// Providers common util function.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// convert string to byte[].
        /// </summary>
        /// <param name="s">target string</param>
        /// <returns>byte[]</returns>
        public static byte[] bytes(string s)
        {
            return Encoding.Default.GetBytes(s);
        }

        /// <summary>
        /// convert byte[] to string.
        /// </summary>
        /// <param name="bys">byte[]</param>
        /// <returns>string</returns>
        public static string tos(byte[] bys)
        {
            return Encoding.UTF8.GetString(bys);
        }

        /// <summary>
        /// convert byte[] to string.
        /// </summary>
        /// <param name="bys">byte[] data.</param>
        /// <param name="off">data offset</param>
        /// <param name="len">data length</param>
        /// <returns></returns>
        public static string tos(byte[] bys, int off, int len)
        {
            return Encoding.UTF8.GetString(bys, off, len);
        }

        /// <summary>
        /// compare two byte[] value
        /// </summary>
        /// <param name="a">one byte[]</param>
        /// <param name="b">other byte[]</param>
        /// <returns>return true if all byte value is equal</returns>
        public static bool equal(byte[] a, byte[] b)
        {
            if (a == null)
            {
                return b == null;
            }
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static IDictionary<string, object> dict(string key, object val)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            res.Add(key, val);
            return res;
        }

        public static IDictionary<string, object> NewDict()
        {
            return new Dictionary<string, object>();
        }

        public static long Now()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static String read(String file)
        {
            using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
    }
}

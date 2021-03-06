﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

        public static string tos(ICollection<String> cs)
        {
            return String.Join(",", cs.ToArray());
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
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        public static String read(String file)
        {
            using (StreamReader reader = new StreamReader(file, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static void write(String file, String data)
        {
            using (StreamWriter reader = new StreamWriter(file, true, Encoding.UTF8))
            {
                reader.Write(data);
            }
        }

        private static Int64 uuid_ = 0;
        public static String UUID()
        {
            string name = Dns.GetHostName();
            name += Process.GetCurrentProcess().Id;
            name += Now();
            name += Interlocked.Increment(ref uuid_);
            return Sha1(name);
        }

        public static string Sha1(byte[] data)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] sha_b = sha1.ComputeHash(data);
            string sha_s = BitConverter.ToString(sha_b);
            return sha_s.ToUpper();
        }
        public static string Sha1(String data)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] sha_b = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
            string sha_s = BitConverter.ToString(sha_b);
            return sha_s.ToUpper();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static void SaveThumbnail(Bitmap bm, String spath, int maxw, int maxh, bool dispose = false, bool whitebackground = false, String ext = ".JPG")
        {
            var scale = 1f;
            var tw = bm.Width;
            var th = bm.Height;
            if (tw > maxw)
            {
                tw = maxw;
                scale = (float)maxw / (float)bm.Width;
                th = (int)(bm.Height * scale);
            }
            if (th > maxh)
            {
                th = maxh;
                scale = (float)maxh / (float)bm.Height;
                tw = (int)(bm.Width * scale);
            }
            var thumb_i = bm.GetThumbnailImage(tw, th, () => { return false; }, IntPtr.Zero);
            if (dispose)
            {
                bm.Dispose();
            }
            var thumb_b = new Bitmap(thumb_i);
            thumb_i.Dispose();
            if (whitebackground)
            {
                var imag_i = thumb_b.GetHbitmap(Color.White);
                thumb_b.Dispose();
                thumb_b = Bitmap.FromHbitmap(imag_i);
                DeleteObject(imag_i);
            }
            var args = new EncoderParameters(1);
            args.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            var enc = ImageCodecInfo.GetImageEncoders()
                            .Where(x => x.FilenameExtension.Contains(ext))
                            .FirstOrDefault();
            thumb_b.Save(spath, enc, args);
            thumb_b.Dispose();
        }

        public static void set<T>(T[] data, T val)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = val;
            }
        }

        public static bool IsOrignialType(Type t)
        {
            return t.FullName.StartsWith("System.") &&
                !t.FullName.StartsWith("System.Collections") &&
                !t.FullName.EndsWith("[]");
        }

        public static bool IsImpl(Type type, Type impl)
        {
            foreach (var i in type.GetInterfaces())
            {
                if (i == impl)
                {
                    return true;
                }
                if (i.IsGenericTypeDefinition)
                {
                    if (i.GetGenericTypeDefinition() == impl)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

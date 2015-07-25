using System;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers converter to convert byte[] to object.
    /// </summary>
    public interface Converter
    {
        /// <summary>
        /// convert byte[] to object.
        /// </summary>
        /// <typeparam name="T">object type.</typeparam>
        /// <param name="nw">base stream</param>
        /// <param name="bys">byte[]</param>
        /// <returns></returns>
        T B2V<T>(Netw nw, Bys bys);

        /// <summary>
        /// convert object to byte[].
        /// </summary>
        /// <param name="nv">base stream</param>
        /// <param name="v">object</param>
        /// <returns></returns>
        Bys V2B(Netw nv, Object v);
    }
}
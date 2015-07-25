using System;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Bys is byte[] slice.
    /// </summary>
    public interface Bys : Netw, NetwVable
    {
        /// <summary>
        /// byte[] slice length.
        /// </summary>
        int length { get; }
        /// <summary>
        /// byte[] slice offset.
        /// </summary>
        int offset { get; }

        /// <summary>
        /// original byte[].
        /// </summary>
        byte[] bys { get; }

        /// <summary>
        /// copy byte[] slice.
        /// </summary>
        byte[] sbys { get; }

        /// <summary>
        /// reset the slice offset and length basing orignal byte[].
        /// </summary>
        /// <param name="off">offset</param>
        /// <param name="len">length</param>
        void reset(int off, int len);

        /// <summary>
        /// forward slice by length basing current offset and length.
        /// </summary>
        /// <param name="len">length</param>
        void forward(int len);

        /// <summary>
        /// create slice by offset basing current offset and length..
        /// </summary>
        /// <param name="off"></param>
        /// <returns>new slice</returns>
        Bys slice(int off);

        /// <summary>
        /// create slice by offset and length basing current offset and length.
        /// </summary>
        /// <param name="off">offset</param>
        /// <param name="len">length</param>
        /// <returns>new slice</returns>
        Bys slice(int off, int len);

        /// <summary>
        /// getting byte by index.
        /// </summary>
        /// <param name="idx">index</param>
        /// <returns>byte value</returns>
        byte get(int idx);

        /// <summary>
        /// getting one short integer by offset basing current offset.
        /// </summary>
        /// <param name="off">offset</param>
        /// <returns>short integer</returns>
        short shortv(int off);

        /// <summary>
        /// covert byte[] to string basing current offset and length.
        /// </summary>
        /// <returns>string</returns>
        String toBs();
    }
}

using System.Collections.Generic;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Provides base fucntion to read/write stream by mode head.  
    /// </summary>
    public interface Netw : NetwBase
    {
        /// <summary>
        /// read and wait to full buffer.
        /// </summary>
        /// <param name="buf">buffer</param>
        /// <returns>data length</returns>
        int readw(byte[] buf);

        /// <summary>
        /// read one command data from stream.
        /// it do not contain mode head.
        /// </summary>
        /// <returns>command data</returns>
        byte[] readm();

        /// <summary>
        /// read one command from stream.
        /// it do not contain mode head.
        /// </summary>
        /// <returns>command</returns>
        Bys readM();

        /// <summary>
        /// create command by byte[] data.
        /// </summary>
        /// <param name="m">byte[] data</param>
        /// <param name="off">data offset.</param>
        /// <param name="len">data length.</param>
        /// <returns>command</returns>
        Bys newM(byte[] m, int off, int len);
        Bys newM(NetwBase rw, byte[] m, int off, int len);
        /// <summary>
        /// write one command data to stream.
        /// it will send data by H_MOD|data length|data.
        /// </summary>
        /// <param name="m">command data</param>
        void writem(byte[] m);

        /// <summary>
        /// write one command data to stream.
        /// it will send data by H_MOD|data length|data.
        /// </summary>
        /// <param name="m">command data</param>
        /// <param name="off">data offset</param>
        /// <param name="length">data length</param>
        void writem(byte[] m, int off, int length);

        /// <summary>
        /// write command data list to stream.
        /// it will send data by H_MOD|data length|data0|data1|data2...
        /// </summary>
        /// <param name="ms"></param>
        void writem(IList<byte[]> ms);

        /// <summary>
        /// write one command to stream.
        /// it will send data by H_MOD|data length|data.
        /// </summary>
        /// <param name="m">command</param>
        void writeM(Bys m);
    }
}

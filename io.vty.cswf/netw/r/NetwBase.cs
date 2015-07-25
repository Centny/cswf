using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// the netw base interface to read and write from stream by command.
    /// it will write data by mode head.
    /// mode head:H_MODE|data length with 16 bit integer|data
    /// </summary>
    public interface NetwBase
    {
        public const byte[] H_MOD = {byte('s') };
        /// <summary>
        /// read and wait data to full byte[] by offset and length.
        /// </summary>
        /// <param name="buf">byte buffer</param>
        /// <param name="off">read offset</param>
        /// <param name="len">read data length</param>
        /// <returns>data length</returns>
        int readw(byte[] buf, int off, int len);


    }
}

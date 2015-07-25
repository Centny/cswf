using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Provides base method to read and write from stream by command.
    /// it will write data by mode head.
    /// mode head:H_MODE|data length with 16 bit integer|data
    /// </summary>
    public interface NetwBase
    {
        /// <summary>
        /// read and wait data to full byte[] by offset and length.
        /// </summary>
        /// <param name="buf">byte buffer</param>
        /// <param name="off">read offset</param>
        /// <param name="len">read data length</param>
        /// <returns>data length</returns>
        int readw(byte[] buf, int off, int len);
        /// <summary>
        /// property from write data limit.
        /// </summary>
        int limit { get; set; }

        /// <summary>
        /// write data by mode head.
        /// it will send data by Var.H_MOD|data length|data.
        /// </summary>
        /// <param name="ms">data list</param>
        void writeM(IList<Bys> ms);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// Providers interface to write object to stream.
    /// </summary>
    public interface NetwVer : Netw, Converter
    {
        /// <summary>
        /// write object to stream.
        /// it will send data by H_MOD|data length|data.
        /// </summary>
        /// <param name="v"></param>
        void writev(Object v);
    }
}

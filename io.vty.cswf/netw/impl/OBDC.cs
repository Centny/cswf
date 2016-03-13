using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers one byte distribute connection.
    /// </summary>
    public class OBDC : PrefC
    {

        /// <summary>
        /// default constructor by base stream and byte.
        /// </summary>
        /// <param name="nb">base stream</param>
        /// <param name="m">byte mode</param>
        public OBDC(r.NetwBase nb, byte m) : base(nb, m)
        {
        }

    }
}

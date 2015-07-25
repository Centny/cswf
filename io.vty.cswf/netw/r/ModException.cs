using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.r
{
    /// <summary>
    /// the netw mode exception.
    /// </summary>
    public class ModException : Exception
    {
        /// <summary>
        /// default constructor.
        /// </summary>
        /// <param name="m">message</param>
        public ModException(String m) : base(m)
        {
        }
    }
}

using io.vty.cswf.netw.r;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.netw
{
    /// <summary>
    /// Providers runner which implt B2V/V2B by json.
    /// </summary>
    public abstract class NetwRunnerV_j : NetwRunnerV
    {
        /// <summary>
        /// the inherted constructor
        /// </summary>
        /// <param name="msg">message listener.</param>
        /// <param name="evn">event listener</param>
        public NetwRunnerV_j(CmdListener msg, EvnListener evn) : base(msg, evn)
        {
        }

        public override T B2V<T>(Bys bys)
        {
            return Json.parse<T>(bys.bys, bys.offset, bys.length);
        }

        public override Bys V2B(Netw nv, object v)
        {
            byte[] bys = Json.stringify_(v);
            return this.netw.newM(bys, 0, bys.Length);
        }
    }
}

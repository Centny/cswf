using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.vty.cswf.netw.r;
using io.vty.cswf.log;

namespace io.vty.cswf.netw.impl
{
    /// <summary>
    /// Providers one byte distribute handler.
    /// </summary>
    public class OBDH : r.CmdListener
    {
        private static readonly ILog L = Log.New();

        /// <summary>
        /// the byte handler mapping.
        /// </summary>
        private Dictionary<byte, r.CmdListener> hs = new Dictionary<byte, r.CmdListener>();

        public void onCmd(NetwRunnable nr, Bys m)
        {
            byte mark = m.get(0);
            if (hs.ContainsKey(mark))
            {
                m.forward(1);
                hs[mark].onCmd(nr, m);
            }
            else
            {
                L.W("receive unknow mark({0}) message({1}) on handlers:{2}", mark, m.ToString(), this.hs);
            }
        }

        /// <summary>
        /// adding handler by mode.
        /// </summary>
        /// <param name="m">mode</param>
        /// <param name="h">handler</param>
        public void addh(byte m, CmdListener h)
        {
            this.hs.Add(m, h);
        }
    }
}

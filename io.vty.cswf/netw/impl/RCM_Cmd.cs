using io.vty.cswf.netw.r;
using io.vty.cswf.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.netw.impl
{
    public class RCM_Cmd : Dict
    {
        public Bys cmd;
        public string name;
        public RCM_Cmd(Bys cmd, string name, IDictionary<string, object> data) : base(data)
        {

        }
    }
}

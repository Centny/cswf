using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.vty.cswf.util
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class M2S : Attribute
    {
        public String Name { get; set; }
        public bool Ignore { get; set; }
        public bool Emit { get; set; }
    }
}

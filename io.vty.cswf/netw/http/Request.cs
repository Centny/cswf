using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace io.vty.cswf.netw.http
{
    public class Request : StreamWriter
    {
        public HttpListenerContext ctx { get; protected set; }
        public HttpListenerRequest req { get; protected set; }
        public HttpListenerResponse res { get; protected set; }

        public Request(HttpListenerContext ctx, HttpListenerRequest req, HttpListenerResponse res) : base(res.OutputStream)
        {
            this.ctx = ctx;
            this.req = req;
            this.res = res;
        }
    }
}

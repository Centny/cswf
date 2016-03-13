using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace io.vty.cswf.netw.http
{
    public class Server
    {
        private static readonly ILog L = Log.New();
        public delegate HResult HTTP_H(Request r);
        public HttpListener Listener;
        public bool Running { get; protected set; }
        public IList<KeyValuePair<Regex, HTTP_H>> FS { get; protected set; }
        public IList<KeyValuePair<Regex, HTTP_H>> HS { get; protected set; }

        public Server()
        {
            this.Listener = new HttpListener();
            this.FS = new List<KeyValuePair<Regex, HTTP_H>>();
            this.HS = new List<KeyValuePair<Regex, HTTP_H>>();
        }
        public void AddPrefix(String prefix)
        {
            this.Listener.Prefixes.Add(prefix);
        }
        public void AddF(String regex, HTTP_H f)
        {
            this.FS.Add(new KeyValuePair<Regex, HTTP_H>(new Regex(regex), f));
        }
        public void AddH(String regex, HTTP_H h)
        {
            this.HS.Add(new KeyValuePair<Regex, HTTP_H>(new Regex(regex), h));
        }
        public void Start()
        {
            this.Listener.Start();
            this.Running = true;
            ThreadPool.QueueUserWorkItem(this.run);
        }

        public void Stop()
        {
            this.Running = false;
            this.Listener.Close();
        }

        protected void run(object o)
        {
            L.I("Server starting...");
            try
            {
                while (this.Running)
                {
                    HttpListenerContext context = this.Listener.GetContext();
                    new Task(i =>
                    {
                        this.doRequrest(i as HttpListenerContext);

                    }, context).Start();
                }
            }
            catch (Exception e)
            {
                L.E(e, "Serve is running fail with {0}, it will stop", e.Message);
            }
            L.I("Server is stopped");
        }
        protected void doRequrest(HttpListenerContext ctx)
        {
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse res = ctx.Response;
            var R = new Request(ctx, req, res);
            try
            {
                var path = req.Url.PathAndQuery;
                foreach (var f in this.FS)
                {
                    if (!f.Key.IsMatch(path))
                    {
                        continue;
                    }
                    var hres = f.Value(R);
                    if (hres == HResult.HRES_RETURN)
                    {
                        return;
                    }
                }
                foreach (var h in this.HS)
                {
                    if (h.Key.IsMatch(path))
                    {
                        h.Value(R);
                        return;
                    }
                }
                res.StatusCode = 404;
                var bys = Encoding.UTF8.GetBytes("404");
                res.OutputStream.Write(bys, 0, bys.Length);
            }
            catch (Exception e)
            {
                res.StatusCode = 500;
                var bys = Encoding.UTF8.GetBytes(e.ToString());
                res.OutputStream.Write(bys, 0, bys.Length);
            }
            finally
            {
                res.OutputStream.Close();
            }
        }
    }
}

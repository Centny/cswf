using System;
using System.Collections.Generic;

namespace io.vty.cswf.netw.http.filter
{
    public class CORS
    {

        public IDictionary<String, String> Sites = new Dictionary<String, String>();
        public String[] Methods = new String[] { "GET", "POST" };
        public String[] Headers = new String[] { "Origin", "X-Requested-With", "Content-Type", "Accept" };
        public CORS()
        {
            this.Sites.Add("*", "N");
        }
        public CORS(String site)
        {
            this.Sites.Add(site, "N");
        }
        public virtual HResult Exec(Request r)
        {
            var origin = r.req.Headers.Get("Origin");
            if (String.IsNullOrEmpty(origin))
            {
                return HResult.HRES_CONTINUE;
            }
            if (this.Sites.ContainsKey("*"))
            {
                return this.Allow(r, "*");
            }
            if (this.Sites.ContainsKey(origin))
            {
                return this.Allow(r, origin);
            }
            r.res.StatusCode = 403;
            r.WriteLine(origin + " is not allowed by cors");
            return HResult.HRES_RETURN;

        }

        protected virtual HResult Allow(Request r, String site)
        {
            r.res.AddHeader("Access-Control-Allow-Origin", site);
            if (this.Headers != null && this.Headers.Length > 0)
            {
                r.res.AddHeader("Access-Control-Allow-Headers", String.Join(", ", this.Headers));
            }
            if (this.Methods != null && this.Methods.Length > 0)
            {
                r.res.AddHeader("Access-Control-Allow-Methods", String.Join(", ", this.Methods));
            }
            if (r.req.HttpMethod == "OPTIONS")
            {
                return HResult.HRES_RETURN;
            }
            else
            {
                return HResult.HRES_CONTINUE;
            }
        }
    }
}

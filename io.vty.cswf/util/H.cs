using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace io.vty.cswf.util
{
    public class H
    {
        public class Response
        {
            public string CharacterSet { get; set; }
            //
            // Summary:
            //     Gets the method that is used to encode the body of the response.
            //
            // Returns:
            //     A string that describes the method that is used to encode the body of the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public string ContentEncoding { get; set; }
            //
            // Summary:
            //     Gets the length of the content returned by the request.
            //
            // Returns:
            //     The number of bytes returned by the request. Content length does not include
            //     header information.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public long ContentLength { get; set; }
            //
            // Summary:
            //     Gets the content type of the response.
            //
            // Returns:
            //     A string that contains the content type of the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public string ContentType { get; set; }
            //
            // Summary:
            //     Gets or sets the cookies that are associated with this response.
            //
            // Returns:
            //     A System.Net.CookieCollection that contains the cookies that are associated with
            //     this response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public CookieCollection Cookies { get; set; }
            //
            // Summary:
            //     Gets the headers that are associated with this response from the server.
            //
            // Returns:
            //     A System.Net.WebHeaderCollection that contains the header information returned
            //     with the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public WebHeaderCollection Headers { get; set; }
            //
            // Summary:
            //     Gets a System.Boolean value that indicates whether both client and server were
            //     authenticated.
            //
            // Returns:
            //     true if mutual authentication occurred; otherwise, false.
            public bool IsMutuallyAuthenticated { get; set; }
            //
            // Summary:
            //     Gets the last date and time that the contents of the response were modified.
            //
            // Returns:
            //     A System.DateTime that contains the date and time that the contents of the response
            //     were modified.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public DateTime LastModified { get; set; }
            //
            // Summary:
            //     Gets the method that is used to return the response.
            //
            // Returns:
            //     A string that contains the HTTP method that is used to return the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public string Method { get; set; }
            //
            // Summary:
            //     Gets the version of the HTTP protocol that is used in the response.
            //
            // Returns:
            //     A System.Version that contains the HTTP protocol version of the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public Version ProtocolVersion { get; set; }
            //
            // Summary:
            //     Gets the URI of the Internet resource that responded to the request.
            //
            // Returns:
            //     A System.Uri that contains the URI of the Internet resource that responded to
            //     the request.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public Uri ResponseUri { get; set; }
            //
            // Summary:
            //     Gets the name of the server that sent the response.
            //
            // Returns:
            //     A string that contains the name of the server that sent the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public string Server { get; set; }
            //
            // Summary:
            //     Gets the status of the response.
            //
            // Returns:
            //     One of the System.Net.HttpStatusCode values.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public HttpStatusCode StatusCode { get; set; }
            //
            // Summary:
            //     Gets the status description returned with the response.
            //
            // Returns:
            //     A string that describes the status of the response.
            //
            // Exceptions:
            //   T:System.ObjectDisposedException:
            //     The current instance has been disposed.
            public string StatusDescription { get; set; }

            public string Data { get; set; }

            public Response(HttpWebResponse res)
            {
                this.CharacterSet = res.CharacterSet;
                this.ContentEncoding = res.ContentEncoding;
                this.ContentLength = res.ContentLength;
                this.ContentType = res.ContentType;
                this.Cookies = res.Cookies;
                this.Headers = res.Headers;
                this.IsMutuallyAuthenticated = res.IsMutuallyAuthenticated;
                this.LastModified = res.LastModified;
                this.Method = res.Method;
                this.ProtocolVersion = res.ProtocolVersion;
                this.ResponseUri = res.ResponseUri;
                this.Server = res.Server;
                this.StatusCode = res.StatusCode;
                this.StatusDescription = res.StatusDescription;
                using (Stream res_s = res.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(res_s, Encoding.UTF8))
                    {
                        this.Data = reader.ReadToEnd();
                    }

                }
            }

            public T parse<T>()
            {
                return new JavaScriptSerializer().Deserialize<T>(this.Data);
            }

            public IDictionary<string,object> toDict()
            {
                return parse<Dictionary<string, object>>();
            }
        }
        public static Response doGet(Dictionary<string, string> headers, string url_f, params Object[] args)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format(url_f, args));
            req.Method = "GET";
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    req.Headers.Add(key, headers[key]);
                }
            }
            return new Response((HttpWebResponse)req.GetResponse());
        }
        public static Response doGet( string url_f, params Object[] args)
        {
            return doGet(null, url_f, args);
        }
    }
}

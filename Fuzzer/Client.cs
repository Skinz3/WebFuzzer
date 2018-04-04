using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fuzzer
{
    public class Client : WebClient
    {
        public bool HeadOnly { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            return req;
        }
        public bool IsUrlValid(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                  && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public HttpStatusCode? GetHttpResponseCode(string url)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            var request = HttpWebRequest.CreateHttp(uriBuilder.Uri);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode;

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    return ((HttpWebResponse)ex.Response).StatusCode;
                }
                else
                    return null;
            }
            finally
            {
                request.Abort();
            }
        }
    }
}

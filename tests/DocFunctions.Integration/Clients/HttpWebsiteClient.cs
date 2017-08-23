using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients
{
    public class HttpWebsiteClient : IWebsiteClient
    {
        private static HttpClient client = new HttpClient();

        public bool UrlExists(string url)
        {
            var result = client.GetAsync(url).Result;
            return (result.StatusCode == HttpStatusCode.OK);
        }

        public bool UrlNotFound(string url)
        {
            var result = client.GetAsync(url).Result;
            return (result.StatusCode == HttpStatusCode.NotFound);
        }

        public long UrlSize(string url)
        {
            var result = client.GetAsync(url).Result;
            var length = result.Content.Headers.ContentLength;

            return length == null ? 0 : (long)length;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Helpers
{
    public class HttpHelpers
    {
        private static HttpClient client = new HttpClient();

        public static bool Exists(string url)
        {
            var result = client.GetAsync(url).Result;
            return (result.StatusCode == HttpStatusCode.OK);
        }

        public static bool NotFound(string url)
        {
            var result = client.GetAsync(url).Result;
            return (result.StatusCode == HttpStatusCode.NotFound);
        }
    }
}

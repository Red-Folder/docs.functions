using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace docsFunctions.Shared.Models
{
    public class Redirect
    {
        public HttpStatusCode RedirectType { get; set; }
        public string Url { get; set; }
        public bool RedirectByRoute { get; set; }
        public bool RedirectByParameter { get; set; }
    }
}

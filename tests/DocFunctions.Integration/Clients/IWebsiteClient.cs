using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients
{
    public interface IWebsiteClient
    {
        bool UrlExists(string url);
        bool UrlNotFound(string url);
        bool UrlSize(string url);
    }
}

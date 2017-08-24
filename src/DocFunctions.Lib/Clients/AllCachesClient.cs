using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
    public class AllCachesClient : IWebCache
    {
        private List<IWebCache> _caches;

        public AllCachesClient(List<IWebCache> caches)
        {
            _caches = caches;
        }

        public void RemoveCachedInstances(string url)
        {
            if (_caches != null)
            {
                _caches.ForEach(x => x.RemoveCachedInstances(url));
            }
        }
    }
}

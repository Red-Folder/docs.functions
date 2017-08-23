using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeFtpsClient : IFtpsClient
    {
        private LocalFakeDataManager _dataManager;

        public LocalFakeFtpsClient(LocalFakeDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public void Delete(string filename)
        {
            throw new NotImplementedException();
        }

        public void Upload(string filename, byte[] contents)
        {
            _dataManager.AddImageToWebsite(filename);
        }

        public void Upload(string filename, string contents)
        {
            _dataManager.AddBlogToWebsite(filename);
        }
    }
}

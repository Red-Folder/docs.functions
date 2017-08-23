using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeGithubClient : IGithubReader
    {
        private LocalFakeDataManager _dataManager;

        public LocalFakeGithubClient(LocalFakeDataManager dataManager)
        {
            _dataManager = dataManager;
        }


        public string GetRawFile(string path, string commitSha)
        {
            throw new NotImplementedException();
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            throw new NotImplementedException();
        }
    }
}

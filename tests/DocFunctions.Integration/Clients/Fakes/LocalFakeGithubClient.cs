using System.Threading.Tasks;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeGithubClient : IGithubReader
    {
        private LocalFakeDataManager _dataManager;

        public LocalFakeGithubClient(LocalFakeDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public Task<Commit> BuildCommitForFullRepoSync()
        {
            throw new System.NotImplementedException();
        }

        public string GetRawFile(string path, string commitSha)
        {
            return _dataManager.GetRawFile(path, commitSha);
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            return _dataManager.GetRawImageFile(path, commitSha);
        }
    }
}

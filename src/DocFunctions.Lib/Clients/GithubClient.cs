using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
    public class GithubClient : IGithubReader
    {
        private string _username;
        private string _key;
        private string _repo;

        public GithubClient(string username, string key, string repo)
        {
            _username = username;
            _key = key;
            _repo = repo;
        }

        public string GetRawFile(string path)
        {
            var client = GetClient();

            var contents = client.Repository.Content.GetAllContents("red-folder", "red-folder.docs.staging", path).Result;

            return contents.First().Content;
        }

        private Octokit.GitHubClient GetClient()
        {
            var credentials = new Octokit.Credentials(_username, _key);
            var connection = new Octokit.Connection(new Octokit.ProductHeaderValue("Red-Folder.DocFunctions"))
            {
                Credentials = credentials
            };
            return new Octokit.GitHubClient(connection);
        }
    }
}

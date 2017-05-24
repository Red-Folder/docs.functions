using DocFunctions.Lib.Wappers;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DocFunctions.Lib.Clients
{
    [ExcludeFromCodeCoverage]
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

            return GetContents(client, path).Content;
        }

        public byte[] GetRawImageFile(string path)
        {
            var client = GetClient();

            var encodedContent = GetContents(client, path).EncodedContent;

            // Convert Base64 String to byte[]
            return Convert.FromBase64String(encodedContent);
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

        private RepositoryContent GetContents(Octokit.GitHubClient client, string path)
        {
            var contents = client.Repository.Content.GetAllContents("red-folder", "red-folder.docs.staging", path).Result;

            return contents.First();
        }
    }
}

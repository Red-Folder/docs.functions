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
        private Octokit.GitHubClient _client;
        private string _username;
        private string _repo;

        public GithubClient(string username, string key, string repo)
        {
            _username = username;
            _repo = repo;

            var credentials = new Octokit.Credentials(_username, key);
            var connection = new Octokit.Connection(new Octokit.ProductHeaderValue("Red-Folder.DocFunctions"))
            {
                Credentials = credentials
            };
            _client = new Octokit.GitHubClient(connection);
        }

        public string GetRawFile(string path, string commitSha)
        {
            return GetContents(_client, path, commitSha).Content;
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            var encodedContent = GetContents(_client, path, commitSha).EncodedContent;

            // Convert Base64 String to byte[]
            return Convert.FromBase64String(encodedContent);
        }

        private RepositoryContent GetContents(Octokit.GitHubClient client, string path, string commitSha)
        {
            var contents = string.IsNullOrEmpty(commitSha) ?
                            client.Repository.Content.GetAllContents(_username, _repo, path).Result :
                            client.Repository.Content.GetAllContentsByRef(_username, _repo, path, commitSha).Result;

            if (contents.Count() == 0)
            {
                throw new ContentNotFoundException($"Unable to get contents for {path}, commit: {commitSha}");
            }

            return contents.First();
        }

        public async Task<Models.Github.Commit> BuildCommitForFullRepoSync()
        {
            var commit = new Models.Github.Commit();
            commit.Message = "Full repo sync";

            var folders = await _client.Repository.Content.GetAllContents(_username, _repo);

            var shalist = new List<string>();

            foreach (var folder in folders.Where(x => x.Type == ContentType.Dir))
            {
                var files = await _client.Repository.Content.GetAllContents(_username, _repo, folder.Path);
                
                foreach (var file in files.Where(x => x.Type == ContentType.File))
                {
                    if (file.Name.ToLower().EndsWith(".json") ||
                        file.Name.ToLower().EndsWith(".md") ||
                        file.Name.ToLower().EndsWith(".png") ||
                        file.Name.ToLower().EndsWith(".jpg"))
                    {
                        commit.Added.Add(new Models.Github.Added
                        {
                            FullFilename = file.Path
                        });
                    }
                }
            }

            return commit;
        }

        public class ContentNotFoundException : Exception
        {
            public ContentNotFoundException(string message): base(message)
            {

            }
        }
    }
}

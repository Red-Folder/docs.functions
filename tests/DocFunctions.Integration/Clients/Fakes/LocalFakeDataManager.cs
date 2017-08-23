using DocFunctions.Lib.Models.Github;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Specialized;
using System.IO;
using docsFunctions.Shared.Models;
using DocFunctions.Integration.Clients.Wrappers;
using System.Text.RegularExpressions;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeDataManager
    {
        private string NO_PREVIOUS_COMMIT = Guid.NewGuid().ToString();

        private OrderedDictionary _commits = new OrderedDictionary();

        private List<string> _website = new List<string>();

        private List<Blog> _repo = new List<Blog>();

        private AssetReader _assetReader;

        public LocalFakeDataManager(AssetReader assetReader)
        {
            _assetReader = assetReader;
        }

        public WebhookData GetGithubWebhookData(Models.Commit commit)
        {
            var newCommitSha = Guid.NewGuid();
            var previousCommitSha = _commits.Count == 0 ? NO_PREVIOUS_COMMIT : _commits[_commits.Count - 1];

            _commits.Add(newCommitSha.ToString(), commit);

            var toBeAdded = commit.ToAdd.Select(x => new Added
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = newCommitSha.ToString()
            }).ToList();
            var toBeModified = commit.ToModify.Select(x => new Modified
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();
            var toBeDeleted = commit.ToDelete.Select(x => new Removed
            {
                FullFilename = x.RepoFilename,
                CommitSha = newCommitSha.ToString(),
                CommitShaForRead = previousCommitSha.ToString()
            }).ToList();

            WebhookData data = new WebhookData
            {
                Commits = new List<Commit>
                {
                    new Commit
                    {
                        Added = toBeAdded,
                        Modified = toBeModified,
                        Removed = toBeDeleted
                    }
                }
            };

            return data;
        }

        public string GetRawFile(string path, string commitSha)
        {
            var sourceFilename = GetSourceFilename(path, commitSha);

            return _assetReader.GetTextFile(sourceFilename);
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            var sourceFilename = GetSourceFilename(path, commitSha);

            return _assetReader.GetImageFile(sourceFilename);
        }

        private string GetSourceFilename(string path, string commitSha)
        {
            if (IsAdd(path, commitSha)) return GetAddSourceFilename(path, commitSha);
            if (IsModify(path, commitSha)) return GetModifySourceFilename(path, commitSha);

            throw new Exception("Unable to find source file");
        }

        private bool IsAdd(string path, string commitSha)
        {
            var commit = (Models.Commit)_commits[commitSha];
            return commit.ToAdd.Any(x => x.RepoFilename == path);
        }

        private bool IsModify(string path, string commitSha)
        {
            var commit = (Models.Commit)_commits[commitSha];
            return commit.ToModify.Any(x => x.RepoFilename == path);
        }

        private bool IsDelete(string path, string commitSha)
        {
            var commit = (Models.Commit)_commits[commitSha];
            return commit.ToDelete.Any(x => x.RepoFilename == path);
        }

        private string GetAddSourceFilename(string path, string commitSha)
        {
            var commit = (Models.Commit)_commits[commitSha];
            return commit.ToAdd.Where(x => x.RepoFilename == path).First().SourceFilename;
        }

        private string GetModifySourceFilename(string path, string commitSha)
        {
            var commit = (Models.Commit)_commits[commitSha];
            return commit.ToModify.Where(x => x.RepoFilename == path).First().SourceFilename;
        }

        public void AddBlogToWebsite(string filename)
        {
            var urlSuffix = filename
                                .Replace("/site/contentroot/", "")
                                .Replace(".html", "");
            var url = $"https://rfc-doc-functions-staging.azurewebsites.net/api/Blog/{urlSuffix}";
            _website.Add(url.ToLower());
        }

        public void AddImageToWebsite(string filename)
        {
            var url = $"http://rfc-website-staging.azurewebsites.net/media/blog/{filename.Replace("/site/mediaroot/blog/", "")}";
            _website.Add(url.ToLower());
        }

        public bool UrlExists(string url)
        {
            // Remove any parameters after the url
            var cleanUrl = url.Split('?')[0];
            return _website.Any(x => x == cleanUrl.ToLower());
        }

        public void SaveBlogToRepo(Blog blogMeta)
        {
            _repo.Add(blogMeta);
        }
    }
}

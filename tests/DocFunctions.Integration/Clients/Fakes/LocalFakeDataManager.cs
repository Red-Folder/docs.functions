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

        private string _lastCommit;
        private Dictionary<string, Models.Commit> _commits = new Dictionary<string, Models.Commit>();

        private List<WebsiteItem> _website = new List<WebsiteItem>();

        private List<Blog> _repo = new List<Blog>();

        private AssetReader _assetReader;

        public LocalFakeDataManager(AssetReader assetReader)
        {
            _assetReader = assetReader;
            _lastCommit = NO_PREVIOUS_COMMIT;
        }

        public WebhookData GetGithubWebhookData(Models.Commit commit)
        {
            var newCommitSha = Guid.NewGuid();
            var previousCommitSha = _lastCommit;

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

            _lastCommit = newCommitSha.ToString();

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
            var commit = _commits[commitSha];
            return commit.ToAdd.Any(x => x.RepoFilename == path);
        }

        private bool IsModify(string path, string commitSha)
        {
            var commit = _commits[commitSha];
            return commit.ToModify.Any(x => x.RepoFilename == path);
        }

        private bool IsDelete(string path, string commitSha)
        {
            var commit = _commits[commitSha];
            return commit.ToDelete.Any(x => x.RepoFilename == path);
        }

        private string GetAddSourceFilename(string path, string commitSha)
        {
            var commit = _commits[commitSha];
            return commit.ToAdd.Where(x => x.RepoFilename == path).First().SourceFilename;
        }

        private string GetModifySourceFilename(string path, string commitSha)
        {
            var commit = _commits[commitSha];
            return commit.ToModify.Where(x => x.RepoFilename == path).First().SourceFilename;
        }

        public void AddBlogToWebsite(string filename, string content)
        {
            _website.Add(new WebsiteItem(filename, content));
        }

        public void AddImageToWebsite(string filename, byte[] content)
        {
            _website.Add(new WebsiteItem(filename, content));
        }

        public bool UrlExists(string url)
        {
            // Remove any parameters after the url
            var cleanUrl = url.Split('?')[0];
            return _website.Any(x => x.FullUrl == cleanUrl.ToLower());
        }

        public void SaveBlogToRepo(Blog blogMeta)
        {
            _repo.Add(blogMeta);
        }

        public void DeleteFromWebsite(string filename)
        {
            _website.RemoveAll(x => x.PhysicalFilename == filename);
        }

        public void DeleteBlogFromRepo(string blogUrl)
        {
            _repo.RemoveAll(x => x.Url == blogUrl);
        }

        public class WebsiteItem
        {
            private string _physicalFilename;
            private Object _contents;

            public WebsiteItem(string physicalFilename, Object contents)
            {
                _physicalFilename = physicalFilename;
                _contents = contents;
            }

            public string FullUrl
            {
                get
                {
                    if (IsImage)
                    {
                        var url = $"http://rfc-website-staging.azurewebsites.net/media/blog/{_physicalFilename.Replace("/site/mediaroot/blog/", "")}";
                        return url.ToLower();
                    }
                    else
                    {
                        var urlSuffix = _physicalFilename
                                            .Replace("/site/contentroot/", "")
                                            .Replace(".html", "");
                        var url = $"https://rfc-doc-functions-staging.azurewebsites.net/api/Blog/{urlSuffix}";
                        return url.ToLower();
                    }
                }
            }

            public bool IsImage
            {
                get
                {
                    return (_physicalFilename.ToLower().EndsWith(".png") || _physicalFilename.ToLower().EndsWith(".jpg"));
                }
            }

            public string PhysicalFilename
            {
                get
                {
                    return _physicalFilename;
                }
            }

            public Object Contents
            {
                get
                {
                    return _contents;
                }
            }
        }
    }
}

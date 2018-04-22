using DocFunctions.Lib.Models.Github;
using System.Collections.Generic;
using System.Linq;
using System;
using docsFunctions.Shared.Models;
using DocFunctions.Integration.Clients.Wrappers;
using DocFunctions.Integration.Models;

namespace DocFunctions.Integration.Clients.Fakes
{
    public class LocalFakeDataManager
    {
        private LocalFakeWebsiteDataManager _websiteManager;
        private LocalFakeRepoDataManager _repoManager;
        private LocalFakeGithubDataManager _githubManager;

        private AssetReader _assetReader;

        public LocalFakeDataManager(AssetReader assetReader)
        {
            _websiteManager = new LocalFakeWebsiteDataManager();
            _repoManager = new LocalFakeRepoDataManager();
            _githubManager = new LocalFakeGithubDataManager(assetReader);
        }

        public WebhookData GetGithubWebhookData(ToBeCommitted commit)
        {
            return _githubManager.GetGithubWebhookData(commit);
        }

        public string GetRawFile(string path, string commitSha)
        {
            return _githubManager.GetRawFile(path, commitSha);
        }

        public byte[] GetRawImageFile(string path, string commitSha)
        {
            return _githubManager.GetRawImageFile(path, commitSha);
        }

        public void AddBlogToWebsite(string filename, string content)
        {
            _websiteManager.AddBlogToWebsite(filename, content);
        }

        public void AddImageToWebsite(string filename, byte[] content)
        {
            _websiteManager.AddImageToWebsite(filename, content);
        }

        public bool UrlExists(string url)
        {
            if (url.Contains("/api/Blog"))
            {
                return _repoManager.UrlExists(url);
            }
            else
            {
                return _websiteManager.UrlExists(url);
            }
        }

        public long UrlSize(string url)
        {
            return _websiteManager.UrlSize(url);
        }

        public string UrlContent(string url)
        {
            if (url.Contains("/api/Blog"))
            {
                return _repoManager.UrlContent(url);
            }
            else
            {
                return _websiteManager.UrlContent(url);
            }
        }

        public void DeleteFromWebsite(string filename)
        {
            _websiteManager.DeleteFromWebsite(filename);
        }

        public void SaveBlogToRepo(Blog blogMeta)
        {
            _repoManager.SaveBlogToRepo(blogMeta);
        }

        public void DeleteBlogFromRepo(string blogUrl)
        {
            _repoManager.DeleteBlogFromRepo(blogUrl);
        }
    }
}

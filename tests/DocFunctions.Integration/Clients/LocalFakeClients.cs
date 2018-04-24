using System;
using DocFunctions.Integration.Clients.Fakes;
using DocFunctions.Integration.Clients.Wrappers;
using DocFunctions.Integration.Models;
using DocFunctions.Lib;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Processors;
using DocFunctions.Lib.Wappers;

namespace DocFunctions.Integration.Clients
{
    public class LocalFakeClients : IRepoClient, IWebsiteClient
    {
        private LocalFakeDataManager _dataManager;
        private ToBeCommitted _toBeCommitted = new ToBeCommitted();

        public LocalFakeClients(AssetReader assetReader)
        {
            _dataManager = new LocalFakeDataManager(assetReader);
        }

        public void AddFileToCommit(string repoFilename, string sourceFilename)
        {
            _toBeCommitted.ToAdd.Add(new ToBeAdded(repoFilename, sourceFilename));
        }

        public void DeleteFileFromCommit(string repoFilename)
        {
            _toBeCommitted.ToDelete.Add(new ToBeDeleted(repoFilename));
        }

        public void ModifyFileInCommit(string repoFilename, string sourceFilename)
        {
            _toBeCommitted.ToModify.Add(new ToBeModified(repoFilename, sourceFilename));
        }

        public void PushCommit(string commitMessage)
        {
            EmulateGithubWebhookFunction();

            // And clear the toBeCommitted
            _toBeCommitted = new Models.ToBeCommitted();
        }

        public bool UrlExists(string url)
        {
            return _dataManager.UrlExists(url);
        }

        public bool UrlNotFound(string url)
        {
            return !UrlExists(url);
        }

        public long UrlSize(string url)
        {
            return _dataManager.UrlSize(url);
        }

        public string GetContent(string url)
        {
            return _dataManager.UrlContent(url);
        }

        private void EmulateGithubWebhookFunction()
        {
            var fakeGithubReader = new LocalFakeGithubClient(_dataManager);
            var markdownProcessor = new MarkdownProcessor();
            var fakeBlobClient = new LocalFakeBlobClient(_dataManager);
            var blogMetaProcessor = new BlogMetaProcessor();
            var fakeBlogMetaRepository = new LocalFakeBlogMetaRepository(_dataManager);
            var cache = new AllCachesClient(null);
            var audit = new AuditTree();
            var actionBuilder = new ActionBuilder(fakeGithubReader, 
                                                    markdownProcessor, 
                                                    fakeBlobClient, 
                                                    blogMetaProcessor, 
                                                    fakeBlogMetaRepository,
                                                    cache,
                                                    audit);

            var webhookAction = new WebhookActionBuilder(actionBuilder, audit);

            var githubWebhookData = _dataManager.GetGithubWebhookData(_toBeCommitted);

            webhookAction.Process(githubWebhookData);
        }
    }
}

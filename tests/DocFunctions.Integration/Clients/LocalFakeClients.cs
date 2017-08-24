using DocFunctions.Integration.Clients.Fakes;
using DocFunctions.Integration.Clients.Wrappers;
using DocFunctions.Integration.Models;
using DocFunctions.Lib;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Processors;

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

        private void EmulateGithubWebhookFunction()
        {
            var fakeGithubReader = new LocalFakeGithubClient(_dataManager);
            var markdownProcessor = new MarkdownProcessor();
            var fakeFtpsClient = new LocalFakeFtpsClient(_dataManager);
            var blogMetaProcessor = new BlogMetaProcessor();
            var fakeBlogMetaRepository = new LocalFakeBlogMetaRepository(_dataManager);
            var actionBuilder = new ActionBuilder(fakeGithubReader, 
                                                    markdownProcessor, 
                                                    fakeFtpsClient, 
                                                    blogMetaProcessor, 
                                                    fakeBlogMetaRepository);

            var webhookAction = new WebhookActionBuilder(actionBuilder);

            var githubWebhookData = _dataManager.GetGithubWebhookData(_toBeCommitted);

            webhookAction.Process(githubWebhookData);
        }
    }
}

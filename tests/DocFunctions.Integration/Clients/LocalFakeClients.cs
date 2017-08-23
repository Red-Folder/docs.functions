using DocFunctions.Integration.Clients.Fakes;
using DocFunctions.Lib;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients
{
    public class LocalFakeClients : IRepoClient, IWebsiteClient
    {
        private LocalFakeDataManager _dataManager = new LocalFakeDataManager();
        private DocFunctions.Integration.Models.Commit _toBeCommitted = new Models.Commit();

        public void AddFileToCommit(string repoFilename, string sourceFilename)
        {
            _toBeCommitted.ToAdd.Add(new Models.Commit.ToBeAdded(repoFilename, sourceFilename));
        }

        public void DeleteFileFromCommit(string repoFilename)
        {
            _toBeCommitted.ToDelete.Add(new Models.Commit.ToBeDeleted(repoFilename));
        }

        public void ModifyFileInCommit(string repoFilename, string sourceFilename)
        {
            _toBeCommitted.ToModify.Add(new Models.Commit.ToBeModified(repoFilename, sourceFilename));
        }

        public void PushCommit(string commitMessage)
        {
            Process();
            // And clear the toBeCommitted
            _toBeCommitted = new Models.Commit();
        }

        public bool UrlExists(string url)
        {
            throw new NotImplementedException();
        }

        public bool UrlNotFound(string url)
        {
            // TODO
            throw new NotImplementedException();
        }

        public long UrlSize(string url)
        {
            throw new NotImplementedException();
        }

        private void Process()
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

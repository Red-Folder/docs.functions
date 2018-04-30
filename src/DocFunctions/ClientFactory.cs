using DocFunctions.Lib;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Processors;
using DocFunctions.Lib.Wappers;
using DocFunctions.Markdown;
using Microsoft.Azure.WebJobs.Host;
using Serilog;
using Serilog.Sinks.AzureWebJobsTraceWriter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DocFunctions
{
    public class ClientFactory
    {
        public IBlogMetaRepository GetBlogMetaRepository { get; private set; }

        public static IGithubReader GetGitHubClient()
        {
            var gitUsername = ConfigurationManager.AppSettings["github-username"];
            var gitKey = ConfigurationManager.AppSettings["github-key"];
            var gitRepo = ConfigurationManager.AppSettings["github-repo"];

            if (gitUsername == null || gitUsername.Length == 0) throw new InvalidOperationException("github-username not set");
            if (gitKey == null || gitKey.Length == 0) throw new InvalidOperationException("github-key not set");
            if (gitRepo == null || gitRepo.Length == 0) throw new InvalidOperationException("github-repo not set");

            return new GithubClient(gitUsername, gitKey, gitRepo);
        }

        public static IBlobClient GetBlobClient()
        {
            var blobContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];
            var blobConnectionString = ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString;

            if (blobContainerName == null || blobContainerName.Length == 0) throw new InvalidOperationException("BlobStorageContainerName not set");
            if (blobConnectionString == null || blobConnectionString.Length == 0) throw new InvalidOperationException("BlobStorage Connection String not set");

            return new AzureBlobClient(blobConnectionString, blobContainerName);
        }

        public static IToBeProcessed GetToBeProcessedClient()
        {
            var queueConnectionString = ConfigurationManager.ConnectionStrings["ToBeProcessedStorage"].ToString();
            var queueContainerName = ConfigurationManager.AppSettings["ToBeProcessedContainerName"];
            var queueQueueName = ConfigurationManager.AppSettings["ToBeProcessedQueueName"];

            if (queueConnectionString == null || queueConnectionString.Length == 0) throw new InvalidOperationException("ToBeProcessedStorage Connection String not set");
            if (queueContainerName == null || queueContainerName.Length == 0) throw new InvalidOperationException("ToBeProcessed Container Name not set");
            if (queueQueueName == null || queueQueueName.Length == 0) throw new InvalidOperationException("ToBeProcessedStorage Queue Name not set");

            return new ToBeProcessedQueue(queueConnectionString, queueContainerName, queueQueueName); 
        }

        public static WebhookActionBuilder GetActionBuild(AuditTree audit)
        {
            var actionBuilder = new ActionBuilder(GetGitHubClient(),
                                     new MarkdownTransformer(),
                                     GetBlobClient(),
                                     new BlogMetaProcessor(),
                                     GetMetaClient(),
                                     new AllCachesClient(null),
                                     audit);

            return new WebhookActionBuilder(actionBuilder, audit);
        }

        public static AuditTree GetAuditClient(TraceWriter log)
        {
            return new AuditTree(GetLogger(log));
        }

        private static ILogger GetLogger(TraceWriter log)
        {
            return new LoggerConfiguration()
                .WriteTo.TraceWriter(log)
                .CreateLogger();
        }

        private static BlogMetaRepository GetMetaClient()
        {
            var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

            if (blogMetaContainerName == null || blogMetaContainerName.Length == 0) throw new InvalidOperationException("BlogMetaStorageContainerName not set");
            if (blogMetaConnectionString == null || blogMetaConnectionString.Length == 0) throw new InvalidOperationException("BlogMetaStorage Connection String not set");

            return new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);
        }
    }
}
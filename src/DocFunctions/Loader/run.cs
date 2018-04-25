using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using DocFunctions.Lib;
using DocFunctions.Lib.Utils;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Processors;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.AzureWebJobsTraceWriter;
using DocFunctions.Lib.Wappers;
using SendGrid.Helpers.Mail;
using DocFunctions.Lib.Models.Audit;
using System.Linq;
using System.Diagnostics;

namespace DocFunctions.Functions
{
    public class Loader
    {
        public static void Run(string request, TraceWriter log, out Mail message)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.TraceWriter(log)
                        .CreateLogger();

            Log.Information("Process initiated");

            Log.Information("Deserialising data");
            WebhookData data = JsonConvert.DeserializeObject<WebhookData>(request);
            Log.Information("Deserialised data");

            try
            {
                // Get all settings
                var gitUsername = ConfigurationManager.AppSettings["github-username"];
                var gitKey = ConfigurationManager.AppSettings["github-key"];
                var gitRepo = ConfigurationManager.AppSettings["github-repo"];

                var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
                var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

                var blobContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];
                var blobConnectionString = ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString;

                var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                var emailTo = ConfigurationManager.AppSettings["EmailTo"];

                if (gitUsername == null || gitUsername.Length == 0) throw new InvalidOperationException("github-username not set");
                if (gitKey == null || gitKey.Length == 0) throw new InvalidOperationException("github-key not set");
                if (gitRepo == null || gitRepo.Length == 0) throw new InvalidOperationException("github-repo not set");

                if (blogMetaContainerName == null || blogMetaContainerName.Length == 0) throw new InvalidOperationException("BlogMetaStorageContainerName not set");
                if (blogMetaConnectionString == null || blogMetaConnectionString.Length == 0) throw new InvalidOperationException("BlogMetaStorage Connection String not set");

                if (blobContainerName == null || blobContainerName.Length == 0) throw new InvalidOperationException("BlobStorageContainerName not set");
                if (blobConnectionString == null || blobConnectionString.Length == 0) throw new InvalidOperationException("BlobStorage Connection String not set");

                if (emailFrom == null || emailFrom.Length == 0) throw new InvalidOperationException("EmailFrom not set");
                if (emailTo == null || emailTo.Length == 0) throw new InvalidOperationException("EmailTo not set");

                var messageText = "";
                var requestId = Guid.NewGuid();
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                using (LogContext.PushProperty("RequestID", requestId))
                {
                    var githubReader = new GithubClient(gitUsername, gitKey, gitRepo);
                    var markdownProcessor = new MarkdownProcessor();
                    var blobClient = new AzureBlobClient(blobConnectionString, blobContainerName);
                    var blogMetaProcessor = new BlogMetaProcessor();
                    var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);
                    var cache = new AllCachesClient(null);
                    var audit = new AuditTree();
                    var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, blobClient, blogMetaProcessor, blogMetaRepository, cache, audit);

                    var webhookAction = new WebhookActionBuilder(actionBuilder, audit);

                    //// Act
                    Log.Information("Processing the data");
                    webhookAction.Process(data);
                    Log.Information("Processing complete");

                    // Generate audit report
                    messageText = new AuditAsHtml(audit).ToString();
                }

                stopWatch.Stop();

                // Send email
                message = new Mail
                {
                    Subject = $"RFC Docs result for {requestId}",
                    From = new Email(emailFrom)
                };

                var personalization = new Personalization();
                personalization.AddTo(new Email(emailTo));

                Content content = new Content
                {
                    Type = "text/html",
                    Value = $"<html><body>{messageText}<p>Took {stopWatch.ElapsedMilliseconds} milliseconds</p></body></html>"
                };
                message.AddContent(content);
                message.AddPersonalization(personalization);
            }
            catch (Exception ex)
            {
                log.Error("Function failed", ex);
                throw ex;
            }
        }
    }
}
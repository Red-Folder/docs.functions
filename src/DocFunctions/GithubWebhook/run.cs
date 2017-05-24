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

namespace DocFunctions.Functions
{
    public class GithubWebhook
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("GitHub Webhook initiated");

            try
            {

                // Get all settings
                var appInsightsKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];

                var gitUsername = ConfigurationManager.AppSettings["github-username"];
                var gitKey = ConfigurationManager.AppSettings["github-key"];
                var gitRepo = ConfigurationManager.AppSettings["github-repo"];

                var ftpsHost = ConfigurationManager.AppSettings["ftps-host"];
                var ftpsUsername = ConfigurationManager.AppSettings["ftps-username"];
                var ftpsPassword = ConfigurationManager.AppSettings["ftps-password"];

                var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
                var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

                if (gitUsername == null || gitUsername.Length == 0) throw new InvalidOperationException("github-username not set");
                if (gitKey == null || gitKey.Length == 0) throw new InvalidOperationException("github-key not set");
                if (gitRepo == null || gitRepo.Length == 0) throw new InvalidOperationException("github-repo not set");

                if (ftpsHost == null || ftpsHost.Length == 0) throw new InvalidOperationException("ftps-host not set");
                if (ftpsUsername == null || ftpsUsername.Length == 0) throw new InvalidOperationException("ftps-username not set");
                if (ftpsPassword == null || ftpsPassword.Length == 0) throw new InvalidOperationException("ftps-password not set");

                if (blogMetaContainerName == null || blogMetaContainerName.Length == 0) throw new InvalidOperationException("BlogMetaStorageContainerName not set");
                if (blogMetaConnectionString == null || blogMetaConnectionString.Length == 0) throw new InvalidOperationException("BlogMetaStorage Connection String not set");

                // Setup objects
                ILogger logger = new NullLogger();
                if (appInsightsKey != null && appInsightsKey.Length > 0)
                {
                    logger = new ApplicationInsightsLogger(appInsightsKey);
                }

                var loggerOperation = logger.StartOperation("DocFunctions - GitHub WebHook");
                var githubReader = new GithubClient(gitUsername, gitKey, gitRepo);
                var markdownProcessor = new MarkdownProcessor();
                var ftpsClient = new FtpsClient(ftpsHost, ftpsUsername, ftpsPassword);
                var blogMetaProcessor = new BlogMetaProcessor();
                var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);
                var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, ftpsClient, blogMetaProcessor, blogMetaRepository);
        
                var webhookAction = new WebhookActionBuilder(actionBuilder);

                // Get request body
                WebhookData data = await req.Content.ReadAsAsync<WebhookData>();

                // Act
                webhookAction.Process(data);

                //logger.Info($"Payload: {JsonConvert.SerializeObject(data)}");

                // Extract github comment from request body
                //foreach (var commit in data.commits)
                //{
                //    logger.Info($"Have commit: {commit.sha}");
                //}

                logger.EndOperation(loggerOperation);
                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Error("Function failed", ex);
                throw ex;
            }
        }
    }
}
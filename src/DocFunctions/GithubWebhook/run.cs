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

                var ftpsHostForHtml = ConfigurationManager.AppSettings["ftps-host-for-html"];
                var ftpsUsernameForHtml = ConfigurationManager.AppSettings["ftps-username-for-html"];
                var ftpsPasswordForHtml = ConfigurationManager.AppSettings["ftps-password-for-html"];

                var ftpsHostForImage = ConfigurationManager.AppSettings["ftps-host-for-image"];
                var ftpsUsernameForImage = ConfigurationManager.AppSettings["ftps-username-for-image"];
                var ftpsPasswordForImage = ConfigurationManager.AppSettings["ftps-password-for-image"];

                var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
                var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

                if (gitUsername == null || gitUsername.Length == 0) throw new InvalidOperationException("github-username not set");
                if (gitKey == null || gitKey.Length == 0) throw new InvalidOperationException("github-key not set");
                if (gitRepo == null || gitRepo.Length == 0) throw new InvalidOperationException("github-repo not set");

                if (ftpsHostForHtml == null || ftpsHostForHtml.Length == 0) throw new InvalidOperationException("ftps-host-for-html not set");
                if (ftpsUsernameForHtml == null || ftpsUsernameForHtml.Length == 0) throw new InvalidOperationException("ftps-username-for-html not set");
                if (ftpsPasswordForHtml == null || ftpsPasswordForHtml.Length == 0) throw new InvalidOperationException("ftps-password-for-html not set");

                if (ftpsHostForImage == null || ftpsHostForImage.Length == 0) throw new InvalidOperationException("ftps-host-for-image not set");
                if (ftpsUsernameForImage == null || ftpsUsernameForImage.Length == 0) throw new InvalidOperationException("ftps-username-for-image not set");
                if (ftpsPasswordForImage == null || ftpsPasswordForImage.Length == 0) throw new InvalidOperationException("ftps-password-for-image not set");

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
                var ftpsClientForHtml = new FtpsClient(ftpsHostForHtml, ftpsUsernameForHtml, ftpsPasswordForHtml);
                var ftpsClientForImage = new FtpsClient(ftpsHostForImage, ftpsUsernameForImage, ftpsPasswordForImage);
                var blogMetaProcessor = new BlogMetaProcessor();
                var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);
                var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, ftpsClientForHtml, ftpsClientForImage, blogMetaProcessor, blogMetaRepository);

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
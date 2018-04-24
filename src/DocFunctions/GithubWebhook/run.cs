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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace DocFunctions.Functions
{
    public class GithubWebhook
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.TraceWriter(log)
                        .CreateLogger();

            Log.Information("GitHub Webhook initiated");

            try
            {
                //// Get all settings
                //var gitUsername = ConfigurationManager.AppSettings["github-username"];
                //var gitKey = ConfigurationManager.AppSettings["github-key"];
                //var gitRepo = ConfigurationManager.AppSettings["github-repo"];

                //var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
                //var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

                //var blobContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];
                //var blobConnectionString = ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString;

                //if (gitUsername == null || gitUsername.Length == 0) throw new InvalidOperationException("github-username not set");
                //if (gitKey == null || gitKey.Length == 0) throw new InvalidOperationException("github-key not set");
                //if (gitRepo == null || gitRepo.Length == 0) throw new InvalidOperationException("github-repo not set");

                //if (blogMetaContainerName == null || blogMetaContainerName.Length == 0) throw new InvalidOperationException("BlogMetaStorageContainerName not set");
                //if (blogMetaConnectionString == null || blogMetaConnectionString.Length == 0) throw new InvalidOperationException("BlogMetaStorage Connection String not set");

                //if (blobContainerName == null || blobContainerName.Length == 0) throw new InvalidOperationException("BlobStorageContainerName not set");
                //if (blobConnectionString == null || blobConnectionString.Length == 0) throw new InvalidOperationException("BlobStorage Connection String not set");

                var queueName = "toProcess";
                var connectionString = ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString;

                using (LogContext.PushProperty("RequestID", Guid.NewGuid()))
                {
                    //var githubReader = new GithubClient(gitUsername, gitKey, gitRepo);
                    //var markdownProcessor = new MarkdownProcessor();
                    //var blobClient = new AzureBlobClient(blobConnectionString, blobContainerName);
                    //var blogMetaProcessor = new BlogMetaProcessor();
                    //var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);
                    //var cache = new AllCachesClient(null);
                    //var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, blobClient, blogMetaProcessor, blogMetaRepository, cache);
                    //IEmailClient emailClient = null;


                    // Get request body
                    Log.Information("Getting rawJson from request");
                    var rawJson = await req.Content.ReadAsStringAsync();
                    Log.Information("Received: {rawJson}", rawJson);
                    Log.Information("Converting to WebhookData");
                    WebhookData data = WebhookData.Deserialize(rawJson);
                    Log.Information("Converted - received {CommitCount} commits", data.Commits.Count);

                    // Parse the connection string and return a reference to the storage account.
                    Log.Information("Login to the storage account");
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                    // Create the queue client.
                    Log.Information("Create the queueClient");
                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                    // Retrieve a reference to a container.
                    Log.Information($"Create reference to the ${queueName} queue");
                    CloudQueue queue = queueClient.GetQueueReference(queueName);

                    // Create the queue if it doesn't already exist
                    Log.Information("Create the queue if needed");
                    queue.CreateIfNotExists();

                    // Create a message and add it to the queue.
                    Log.Information("Create the message");
                    CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(data));

                    Log.Information("Add the message");
                    queue.AddMessage(message);

                    Log.Information("Done");
                }
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
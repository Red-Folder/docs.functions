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
                var requestId = Guid.NewGuid();
                using (LogContext.PushProperty("RequestID", requestId))
                {
                    Log.Information("Setting up clients");
                    var queue = ClientFactory.GetToBeProcessedClient();

                    // Get request body
                    Log.Information("Getting rawJson from request");
                    var rawJson = await req.Content.ReadAsStringAsync();
                    Log.Information("Received: {rawJson}", rawJson);
                    Log.Information("Converting to WebhookData");
                    WebhookData data = WebhookData.Deserialize(rawJson);
                    Log.Information("Converted - received {CommitCount} commits", data.Commits.Count);

                    Log.Information("Add commits to the queue");
                    queue.Add(requestId.ToString(), data.Commits);

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
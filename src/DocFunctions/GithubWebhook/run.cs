using System.Net;
using DocFunctions.Lib.Models.Github;
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
            var audit = ClientFactory.GetAuditClient(log);

            var requestId = Guid.NewGuid().ToString();
            audit.BeginContext(requestId);

            audit.Information("GitHub Webhook initiated");

            try
            {

                audit.Information("Setting up clients");
                var queue = ClientFactory.GetToBeProcessedClient();

                // Get request body
                audit.Information("Getting rawJson from request");
                var rawJson = await req.Content.ReadAsStringAsync();
                audit.Information($"Received: {rawJson}");
                audit.Information("Converting to WebhookData");
                WebhookData data = WebhookData.Deserialize(rawJson);
                audit.Information($"Converted - received {data.Commits.Count} commits");

                audit.Information("Add commits to the queue");
                queue.Add(requestId, data.Commits);

                audit.Information("Done");

                audit.EndContext();
                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                audit.Error("Function failed", ex);
                audit.EndContext();
                throw ex;
            }

        }
    }
}
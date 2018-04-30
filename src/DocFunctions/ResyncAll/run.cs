using System.Net;
using DocFunctions.Lib.Models.Github;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

namespace DocFunctions.Functions
{
    public class ResyncAll
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            var audit = ClientFactory.GetAuditClient(log);

            var requestId = Guid.NewGuid().ToString();
            audit.BeginContext(requestId);

            audit.Information("Resync Started");

            try
            {
                audit.Information("Setting up clients");
                var githubReader = ClientFactory.GetGitHubClient();
                var blobClient = ClientFactory.GetBlobClient();
                var queue = ClientFactory.GetToBeProcessedClient();

                audit.Information("Clearing the blob storage");
                blobClient.ClearAll();

                audit.Information("Create the resync commit");
                var data = await githubReader.BuildCommitForFullRepoSync();

                audit.Information("Add resync commit to the queue");
                queue.Add(requestId.ToString(), new List<Commit> { data });

                audit.Information("Done");

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                audit.Error("Function failed", ex);
                throw ex;
            }
        }
    }
}
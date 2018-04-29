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
using System.Collections.Generic;

namespace DocFunctions.Functions
{
    public class ResyncAll
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.TraceWriter(log)
                        .CreateLogger();

            Log.Information("Resync Started");

            try
            {
                var requestId = Guid.NewGuid();

                using (LogContext.PushProperty("RequestID", requestId))
                {
                    Log.Information("Setting up clients");
                    var githubReader = ClientFactory.GetGitHubClient();
                    var blobClient = ClientFactory.GetBlobClient();
                    var queue = ClientFactory.GetToBeProcessedClient();

                    Log.Information("Clearing the blob storage");
                    blobClient.ClearAll();

                    Log.Information("Create the resync commit");
                    var data = await githubReader.BuildCommitForFullRepoSync();

                    Log.Information("Add resync commit to the queue");
                    queue.Add(requestId.ToString(), new List<Commit> { data });

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
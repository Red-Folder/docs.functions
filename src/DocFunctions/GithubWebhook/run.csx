#r "NewtonSoft.Json"

using System.Net;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    log.Info($"Payload: {JsonConvert.SerializeObject(data)}");

    // Extract github comment from request body
    foreach (var commit in data.commits)
    {
        log.Info($"Have commit: {commit.sha}");
    }

    return req.CreateResponse(HttpStatusCode.OK);
}
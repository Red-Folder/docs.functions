#r "NewtonSoft.Json"

using System.Net;
using Newtonsoft.Json;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    TelemetryClient telemetry = new TelemetryClient();
    string key = TelemetryConfiguration.Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

    telemetry.TrackEvent("GitHub Webhook initiated");

    using (var operation = telemetry.StartOperation<RequestTelemetry>("DocFunctions - GitHub WebHook"))
    {
        // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();

        log.Info($"Payload: {JsonConvert.SerializeObject(data)}");

        // Extract github comment from request body
        foreach (var commit in data.commits)
        {
            log.Info($"Have commit: {commit.sha}");
        }

        telemetry.StopOperation(operation);
    }
    return req.CreateResponse(HttpStatusCode.OK);
}
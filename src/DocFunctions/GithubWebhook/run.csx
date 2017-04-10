#r "NewtonSoft.Json"

using System.Net;
using Newtonsoft.Json;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

private static TelemetryClient telemetry = new TelemetryClient();
private static string key = TelemetryConfiguration.Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    //log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");
    log.Info("GitHub Webhook initiated");
    telemetry.TrackEvent("GitHub Webhook initiated");

    using (var operation = telemetry.StartOperation<RequestTelemetry>("DocFunctions - GitHub WebHook"))
    {
        // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();

        telemetry.TrackTrace($"Payload: {JsonConvert.SerializeObject(data)}", SeverityLevel.Information);
        //log.Info($"Payload: {JsonConvert.SerializeObject(data)}");

        // Extract github comment from request body
        foreach (var commit in data.commits)
        {
            telemetry.TrackTrace($"Have commit: {commit.sha}", SeverityLevel.Information);
            //log.Info($"Have commit: {commit.sha}");
        }

        telemetry.StopOperation(operation);
    }
    return req.CreateResponse(HttpStatusCode.OK);
}
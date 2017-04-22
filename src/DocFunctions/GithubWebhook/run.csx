#r "NewtonSoft.Json"
#r "DocFunctions.Lib.dll"

using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using DocFunctions.Lib;
using DocFunctions.Lib.Utils;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Processors;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // Get all settings
    var appSettings = ConfigurationManager.AppSettings;

    var appInsightsKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
    var gitUsername = System.Environment.GetEnvironmentVariable("github-username", EnvironmentVariableTarget.Process);
    var gitKey = System.Environment.GetEnvironmentVariable("github-key", EnvironmentVariableTarget.Process);
    var gitRepo = System.Environment.GetEnvironmentVariable("github-repo", EnvironmentVariableTarget.Process);
    var ftpsHost = System.Environment.GetEnvironmentVariable("ftps-host", EnvironmentVariableTarget.Process);
    var ftpsUsername = System.Environment.GetEnvironmentVariable("ftps-username", EnvironmentVariableTarget.Process);
    var ftpsPassword = System.Environment.GetEnvironmentVariable("ftps-password", EnvironmentVariableTarget.Process);
    var blogMetaConnectionString = System.Environment.GetEnvironmentVariable("BlogMetaStorageConnectionString", EnvironmentVariableTarget.Process);
    var blogMetaContainerName = System.Environment.GetEnvironmentVariable("BlogMetaStorageContainerName", EnvironmentVariableTarget.Process);

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
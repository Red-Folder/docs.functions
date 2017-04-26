#r "DocFunctions.Lib.dll"
#r "docsFunctions.Shared.dll"

using System.Net;
using System.Configuration;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Utils;
using DocFunctions.Lib.Models.Github;
using docsFunctions.Shared.Models;

public static HttpResponseMessage Run(HttpRequestMessage req, string blogUrl, TraceWriter log)
{
    log.Info("Blog Request initiated");

    try
    {

        // Get all settings
        var appInsightsKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
        var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;

        // Setup objects
        ILogger logger = new NullLogger();
        if (appInsightsKey != null && appInsightsKey.Length > 0)
        {
            logger = new ApplicationInsightsLogger(appInsightsKey);
        }

        var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);

        if (blogUrl == "*")
        {
            return req.CreateResponse(HttpStatusCode.OK, blogMetaRepository.Get().ToList());
        }
        else
        {
            var blog = blogMetaRepository.Get(blogUrl);
            if (blog == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.OK, blog);
            }
        }
    }
    catch (Exception ex)
    {
        log.Error("Function failed", ex);
        throw ex;
    }
}

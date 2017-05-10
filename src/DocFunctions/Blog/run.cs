using System.Net;
using System.Configuration;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Utils;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace DocFunctions.Functions
{
    public class Blog
    {
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

                HttpResponseMessage response = null;
                if (blogUrl == "*")
                {
                    return blogMetaRepository.Get().HasValue ? req.CreateResponse(HttpStatusCode.OK, blogMetaRepository.Get().Value) : req.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return blogMetaRepository.Get(blogUrl).HasValue ? req.CreateResponse(HttpStatusCode.OK, blogMetaRepository.Get(blogUrl).Value) : req.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                log.Error("Function failed", ex);
                throw ex;
            }
        }
    }
}
using System.Configuration;
using System;
using Microsoft.Azure.WebJobs.Host;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.AzureWebJobsTraceWriter;
using SendGrid.Helpers.Mail;
using DocFunctions.Lib.Models.Audit;
using System.Diagnostics;

namespace DocFunctions.Functions
{
    public class Loader
    {
        public static void Run(string requestId, TraceWriter log, out Mail message)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.TraceWriter(log)
                        .CreateLogger();

            Log.Information("Process initiated");

            try
            {
                var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                var emailTo = ConfigurationManager.AppSettings["EmailTo"];

                if (emailFrom == null || emailFrom.Length == 0) throw new InvalidOperationException("EmailFrom not set");
                if (emailTo == null || emailTo.Length == 0) throw new InvalidOperationException("EmailTo not set");

                var messageText = "";
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                using (LogContext.PushProperty("RequestID", requestId))
                {
                    Log.Information("Setting up clients");
                    var audit = ClientFactory.GetAuditClient();
                    var builder = ClientFactory.GetActionBuild(audit);
                    var queue = ClientFactory.GetToBeProcessedClient();

                    Log.Information($"Retrieving commit(s) based on request id: {requestId}");
                    var data = queue.Get(requestId);

                    if (data == null)
                    {
                        messageText = "No commit(s) found to process";
                        Log.Information(messageText);
                    }
                    else
                    {
                        Log.Information("Processing the data");
                        builder.Process(data);
                        Log.Information("Processing complete");

                        // Generate audit report
                        messageText = new AuditAsHtml(audit).ToString();
                    }
                }

                stopWatch.Stop();

                // Send email
                message = new Mail
                {
                    Subject = $"RFC Docs result for {requestId}",
                    From = new Email(emailFrom)
                };

                var personalization = new Personalization();
                personalization.AddTo(new Email(emailTo));

                Content content = new Content
                {
                    Type = "text/html",
                    Value = $"<html><body>{messageText}<p>Took {stopWatch.ElapsedMilliseconds} milliseconds</p></body></html>"
                };
                message.AddContent(content);
                message.AddPersonalization(personalization);
            }
            catch (Exception ex)
            {
                log.Error("Function failed", ex);
                throw ex;
            }
        }
    }
}
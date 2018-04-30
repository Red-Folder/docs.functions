using System.Configuration;
using System;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;
using DocFunctions.Lib.Models.Audit;
using System.Diagnostics;

namespace DocFunctions.Functions
{
    public class Loader
    {
        public static void Run(string requestId, TraceWriter log, out Mail message)
        {
            var audit = ClientFactory.GetAuditClient(log);

            audit.BeginContext(requestId);
            audit.Information("Process initiated");

            try
            {
                var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
                var emailTo = ConfigurationManager.AppSettings["EmailTo"];

                if (emailFrom == null || emailFrom.Length == 0) throw new InvalidOperationException("EmailFrom not set");
                if (emailTo == null || emailTo.Length == 0) throw new InvalidOperationException("EmailTo not set");

                var messageText = "";
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                    audit.Information("Setting up clients");
                    
                    var builder = ClientFactory.GetActionBuild(audit);
                    var queue = ClientFactory.GetToBeProcessedClient();

                    audit.Information($"Retrieving commit(s) based on request id: {requestId}");
                    var data = queue.Get(requestId);

                    if (data == null)
                    {
                        messageText = "No commit(s) found to process";
                        audit.Information(messageText);
                    }
                    else
                    {
                        audit.Information("Processing the data");
                        builder.Process(data);

                        audit.Information("Marking the item as processed");
                        queue.MarkCompleted(requestId);

                        audit.Information("Processing complete");

                        // Generate audit report
                        messageText = new AuditAsHtml(audit).ToString();
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
                audit.Error("Function failed", ex);
                throw ex;
            }

            audit.EndContext();
        }
    }
}
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Helpers;
using Xunit;

namespace DocFunctions.Lib.Integration
{
    public class WebhookActionBuilderTests
    {
        [Fact]
        [Trait("Category","Integration")]
        public void EndToEndNewBlogActionRuns()
        {
            // Arrange
            var gitUsername = AppSettings.AppSetting("github-username");
            var gitKey = AppSettings.AppSetting("github-key");
            var gitRepo = AppSettings.AppSetting("github-repo");
            var githubReader = new GithubClient(gitUsername, gitKey, gitRepo);

            var markdownProcessor = new MarkdownProcessor();

            var ftpsHost = AppSettings.AppSetting("ftps-host");
            var ftpsUsername = AppSettings.AppSetting("ftps-username");
            var ftpsPassword = AppSettings.AppSetting("ftps-password");
            var ftpsClient = new FtpsClient(ftpsHost, ftpsUsername, ftpsPassword);

            var blogMetaProcessor = new BlogMetaProcessor();

            var blogMetaConnectionString = AppSettings.AppSetting("BlogMetaStorageConnectionString");
            var blogMetaContainerName = AppSettings.AppSetting("BlogMetaStorageContainerName");
            var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);

            var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, ftpsClient, blogMetaProcessor, blogMetaRepository);

            var sut = new WebhookActionBuilder(actionBuilder);
            var webhookData = new WebhookData
            {
                Commits = new Commit[1]
                {
                    new Commit
                    {
                        Added = new string[2]
                        {
                            "2017-04-10-20-27-54/blog.json",
                            "2017-04-10-20-27-54/blog.md"
                        }
                    }
                }
            };

            // Act
            sut.Process(webhookData);
        }
    }
}

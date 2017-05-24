using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Processors;
using System.Configuration;

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
            var gitUsername = ConfigurationManager.AppSettings["github-username"];
            var gitKey = ConfigurationManager.AppSettings["github-key"];
            var gitRepo = ConfigurationManager.AppSettings["github-repo"];
            var githubReader = new GithubClient(gitUsername, gitKey, gitRepo);

            var markdownProcessor = new MarkdownProcessor();

            var ftpsHostForHtml = ConfigurationManager.AppSettings["ftps-host-for-html"];
            var ftpsUsernameForHtml = ConfigurationManager.AppSettings["ftps-username-for-html"];
            var ftpsPasswordForHtml = ConfigurationManager.AppSettings["ftps-password-for-html"];
            var ftpsClientForHtml = new FtpsClient(ftpsHostForHtml, ftpsUsernameForHtml, ftpsPasswordForHtml);

            var ftpsHostForImage = ConfigurationManager.AppSettings["ftps-host-for-image"];
            var ftpsUsernameForImage = ConfigurationManager.AppSettings["ftps-username-for-image"];
            var ftpsPasswordForImage = ConfigurationManager.AppSettings["ftps-password-for-image"];
            var ftpsClientForImage = new FtpsClient(ftpsHostForImage, ftpsUsernameForImage, ftpsPasswordForImage);


            var blogMetaProcessor = new BlogMetaProcessor();

            var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);

            var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, ftpsClientForHtml, ftpsClientForImage, blogMetaProcessor, blogMetaRepository);

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

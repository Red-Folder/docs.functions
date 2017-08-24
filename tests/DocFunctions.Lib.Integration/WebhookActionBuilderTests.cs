using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Processors;
using System.Collections.Generic;
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

            var ftpsHost = ConfigurationManager.AppSettings["ftps-host"];
            var ftpsUsername = ConfigurationManager.AppSettings["ftps-username"];
            var ftpsPassword = ConfigurationManager.AppSettings["ftps-password"];
            var ftpsClient = new FtpsClient(ftpsHost, ftpsUsername, ftpsPassword);

            var blogMetaProcessor = new BlogMetaProcessor();

            var blogMetaConnectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var blogMetaContainerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var blogMetaRepository = new BlogMetaRepository(blogMetaConnectionString, blogMetaContainerName);

            var actionBuilder = new ActionBuilder(githubReader, markdownProcessor, ftpsClient, blogMetaProcessor, blogMetaRepository);

            var sut = new WebhookActionBuilder(actionBuilder, null);
            var webhookData = new WebhookData
            {
                Commits = new List<Commit>
                {
                    new Commit
                    {
                        Added = new List<Added>
                        {
                            new Added { FullFilename = "2017-04-10-20-27-54/blog.json", CommitSha = "e74f8255d4c8bc010101ec978efb6ee8d6007b44", CommitShaForRead = "e74f8255d4c8bc010101ec978efb6ee8d6007b44" },
                            new Added { FullFilename = "2017-04-10-20-27-54/blog.md", CommitSha = "e74f8255d4c8bc010101ec978efb6ee8d6007b44", CommitShaForRead = "e74f8255d4c8bc010101ec978efb6ee8d6007b44" },
                            new Added { FullFilename = "2017-04-10-20-27-54/Image.jpg", CommitSha = "e74f8255d4c8bc010101ec978efb6ee8d6007b44", CommitShaForRead = "e74f8255d4c8bc010101ec978efb6ee8d6007b44" }
                        },

                        Removed = new List<Removed>()
                    }
                }
            };

            // Act
            sut.Process(webhookData);
        }
    }
}

using DocFunctions.Lib.Clients;
using Xunit;
using System.Configuration;

namespace DocFunctions.Lib.Integration.Clients
{
    public class GithubClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void GetsRawData()
        {
            var username = ConfigurationManager.AppSettings["github-username"];
            var key = ConfigurationManager.AppSettings["github-key"];
            var repo = ConfigurationManager.AppSettings["github-repo"];

            var sut = new GithubClient(username, key, repo);

            var result = sut.GetRawFile("README.md");

            Assert.Contains("A test version of my red-folder.docs repo - used for Staging testing", result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetsRawImage()
        {
            var username = ConfigurationManager.AppSettings["github-username"];
            var key = ConfigurationManager.AppSettings["github-key"];
            var repo = ConfigurationManager.AppSettings["github-repo"];

            var sut = new GithubClient(username, key, repo);

            var result = sut.GetRawImageFile("2017-05-22-20-32-16/Image.jpg");

            Assert.NotNull(result);
        }
    }
}

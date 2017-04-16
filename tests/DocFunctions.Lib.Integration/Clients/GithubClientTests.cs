using DocFunctions.Lib.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Helpers;
using Xunit;

namespace DocFunctions.Lib.Integration.Clients
{
    public class GithubClientTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void GetsRawData()
        {
            var username = AppSettings.AppSetting("github-username");
            var key = AppSettings.AppSetting("github-key");
            var repo = AppSettings.AppSetting("github-repo");

            var sut = new GithubClient(username, key, repo);

            var result = sut.GetRawFile("README.md");

            Assert.Contains("A test version of my red-folder.docs repo - used for Staging testing", result);
        }
    }
}

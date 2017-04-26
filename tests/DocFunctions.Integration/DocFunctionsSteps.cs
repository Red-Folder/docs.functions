using DocFunctions.Integration.Helpers;
using System;
using System.Configuration;
using System.Threading;
using TechTalk.SpecFlow;
using Tests.Common.Helpers;
using Xunit;

namespace DocFunctions.Integration
{
    [Binding]
    public class DocFunctionsSteps
    {
        private string _blogname;

        private string RepoUrl
        {
            get
            {
                return $"https://rfc-doc-functions-staging.azurewebsites.net/api/Blog/{BlogName}?code={AzureFunctionKey}";
            }
        }

        private string BlogName
        {
            get
            {
                return _blogname;
            }
        }

        private string AzureFunctionKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AzureFunctionKey"];
            }
        }

        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
            _blogname = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        [Then(@"I would expect the blog to not be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToNotBeAvailableViaTheBlogAPI()
        {
            Assert.True(HttpHelpers.NotFound(RepoUrl));
        }

        [When(@"I publish a new blog to my Github repo")]
        public void WhenIPublishANewBlogToMyGithubRepo()
        {
            // Generate a new blog name based on the Current Date And Time
            var username = ConfigurationManager.AppSettings["github-username"];
            if (username == null) username = System.Environment.GetEnvironmentVariable("github-username");
            var key = ConfigurationManager.AppSettings["github-key"];
            if (key == null) key = System.Environment.GetEnvironmentVariable("github-key");
            var repo = ConfigurationManager.AppSettings["github-repo"];
            if (repo == null) repo = System.Environment.GetEnvironmentVariable("github-repo");

            var github = new GitHub(username, key, repo, BlogName);
            github.CreateTestBog();
        }

        [Then(@"I allow (.*) seconds")]
        public void ThenIAllowSeconds(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        [Then(@"I would expect the blog to be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToBeAvailableViaTheBlogAPI()
        {
            Assert.True(HttpHelpers.Exists(RepoUrl));
        }

    }
}

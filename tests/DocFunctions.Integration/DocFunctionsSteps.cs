using DocFunctions.Integration.Helpers;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace DocFunctions.Integration
{
    [Binding]
    public class DocFunctionsSteps
    {
        private Config _config;

        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
            _config = new Config(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        }

        [Then(@"I would expect the blog to not be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToNotBeAvailableViaTheBlogAPI()
        {
            Assert.True(HttpHelpers.NotFound(_config.RepoUrl));
        }

        [When(@"I publish a new blog to my Github repo")]
        public void WhenIPublishANewBlogToMyGithubRepo()
        {
            var github = new GitHub(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo, _config.BlogName);
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
            Assert.True(HttpHelpers.Exists(_config.RepoUrl));
        }

        [Then(@"I would expect the image to be available via the website")]
        public void ThenIWouldExpectTheImageToBeAvailableViaTheWebsite()
        {
            Assert.True(HttpHelpers.Exists(_config.ImageUrl));
        }

        [When(@"I delete that blog from my Github repo")]
        public void WhenIDeleteThatBlogFromMyGithubRepo()
        {
            var github = new GitHub(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo, _config.BlogName);
            github.DeleteTestBog();
        }

        [Then(@"I would expect the image to not be available via the website")]
        public void ThenIWouldExpectTheImageToNotBeAvailableViaTheWebsite()
        {
            Assert.True(HttpHelpers.NotFound(_config.ImageUrl));
        }

    }
}

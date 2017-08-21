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

        public DocFunctionsSteps()
        {
            _config = new Config(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        }

        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
        }

        [Then(@"I would expect the blog to not be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToNotBeAvailableViaTheBlogAPI()
        {
            Assert.True(HttpHelpers.NotFound(_config.RepoUrl));
        }

        [Given(@"I publish a new blog to my Github repo")]
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

        [When(@"I update that image")]
        public void WhenIUpdateThatImage()
        {
            var github = new GitHub(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo, _config.BlogName);
            github.UpdateImage();
        }

        [Then(@"I would expect the new image to be available via the website")]
        public void ThenIWouldExpectTheNewImageToBeAvailableViaTheWebsite()
        {
            Assert.Equal(5585, HttpHelpers.FileSize(_config.ImageUrl));
        }

        [When(@"I update that blog text")]
        public void WhenIUpdateThatBlogText()
        {
            var github = new GitHub(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo, _config.BlogName);
            github.UpdateBlogText();
        }

        [Then(@"I would expect the new blog text to be available via the website")]
        public void ThenIWouldExpectTheNewBlogTextToBeAvailableViaTheWebsite()
        {
            Assert.Equal(169, HttpHelpers.FileSize(_config.ImageUrl));
        }

    }
}

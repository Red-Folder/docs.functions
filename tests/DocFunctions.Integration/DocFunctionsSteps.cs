using DocFunctions.Integration.Clients;
using DocFunctions.Integration.Models;
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
        private IRepoClient _repoClient;
        private IWebsiteClient _websiteClient;

        public DocFunctionsSteps()
        {
            _config = new Config(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            var runLocal = true;
            if (runLocal)
            {
                var localFake = new LocalFakeClients();
                _repoClient = localFake;
                _websiteClient = localFake;
            }
            else
            {
                _repoClient = new GithubRepoClient(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo);
                _websiteClient = new HttpWebsiteClient();
            }
        }

        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
        }

        [Then(@"I would expect the blog to not be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToNotBeAvailableViaTheBlogAPI()
        {
            Assert.True(_websiteClient.UrlNotFound(_config.RepoUrl));
        }

        [Then(@"I allow (.*) seconds")]
        public void ThenIAllowSeconds(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        [Then(@"I would expect the blog to be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToBeAvailableViaTheBlogAPI()
        {
            Assert.True(_websiteClient.UrlExists(_config.RepoUrl));
        }

        [Then(@"I would expect the image to be available via the website")]
        public void ThenIWouldExpectTheImageToBeAvailableViaTheWebsite()
        {
            Assert.True(_websiteClient.UrlExists(_config.ImageUrl));
        }

        [Then(@"I would expect the image to not be available via the website")]
        public void ThenIWouldExpectTheImageToNotBeAvailableViaTheWebsite()
        {
            Assert.True(_websiteClient.UrlNotFound(_config.ImageUrl));
        }

        [Then(@"I would expect the new image to be available via the website")]
        public void ThenIWouldExpectTheNewImageToBeAvailableViaTheWebsite()
        {
            Assert.Equal(5585, _websiteClient.UrlSize(_config.ImageUrl));
        }

        [Then(@"I would expect the new blog text to be available via the website")]
        public void ThenIWouldExpectTheNewBlogTextToBeAvailableViaTheWebsite()
        {
            Assert.Equal(169, _websiteClient.UrlSize(_config.ImageUrl));
        }

        [Given(@"I start a new commit")]
        [When(@"I start a new commit")]
        public void WhenIStartANewCommit()
        {
        }

        [Given(@"Add (.*) to the commit")]
        [When(@"Add (.*) to the commit")]
        public void WhenAddToTheCommit(string filename)
        {
            _repoClient.AddFileToCommit(_config.GetRepoFilename(filename), _config.GetAssetFilename(filename));
        }

        [When(@"Delete (.*) from the commit")]
        public void WhenDeleteFromTheCommit(string filename)
        {
            _repoClient.DeleteFileFromCommit(_config.GetRepoFilename(filename));
        }

        [When(@"Replace (.*) with (.*) in the commit")]
        public void WhenReplaceInTheCommit(string original, string replacement)
        {
            _repoClient.ModifyFileInCommit(_config.GetRepoFilename(original), _config.GetAssetFilename(replacement));
        }

        [Given(@"Push the commit with message ""(.*)""")]
        [When(@"Push the commit with message ""(.*)""")]
        public void WhenPushTheCommit(string commitMessage)
        {
            _repoClient.PushCommit(commitMessage);
        }
    }
}

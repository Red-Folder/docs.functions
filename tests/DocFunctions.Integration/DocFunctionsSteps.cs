﻿using DocFunctions.Integration.Clients;
using DocFunctions.Integration.Clients.Wrappers;
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

            var assetReader = new AssetReader(_config);

            if (_config.UseLocalFake)
            {
                var localFake = new LocalFakeClients(assetReader);
                _repoClient = localFake;
                _websiteClient = localFake;
            }
            else
            {
                _repoClient = new GithubRepoClient(_config.GitHubUsername, _config.GitHubKey, _config.GitHubRepo, assetReader);
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
            Assert.True(_websiteClient.UrlNotFound(_config.MetaUrl));
        }

        [Then(@"I allow (.*) seconds")]
        public void ThenIAllowSeconds(int seconds)
        {
            if (!_config.UseLocalFake)
            {
                Thread.Sleep(seconds * 1000);
            }
        }

        [Then(@"I would expect the blog to be available via the Blog API")]
        public void ThenIWouldExpectTheBlogToBeAvailableViaTheBlogAPI()
        {
            Assert.True(_websiteClient.UrlExists(_config.MetaUrl));
        }

        [Then(@"I would expect the blog content to be available via the website")]
        public void ThenIWouldExpectTheBlogContentToBeAvailableViaTheWebsite()
        {
            Assert.True(_websiteClient.UrlExists(_config.ContentUrl));
        }

        [Then(@"I would expect the blog content to not be available via the website")]
        public void ThenIWouldExpectTheBlogContentToNotBeAvailableViaTheWebsite()
        {
            Assert.True(_websiteClient.UrlNotFound(_config.ContentUrl));
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

        [Then(@"I would expect the new blog meta to be available via the website")]
        public void ThenIWouldExpectTheNewBlogTextToBeAvailableViaTheWebsite()
        {
            Assert.Contains("UPDATED", _websiteClient.GetContent(_config.MetaUrl));
        }

        [Then(@"I would expect the new blog content to be available via the website")]
        public void ThenIWouldExpectTheNewBlogContentToBeAvailableViaTheWebsite()
        {
            Assert.Contains("This version has sooooo much more text", _websiteClient.GetContent(_config.ContentUrl));
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

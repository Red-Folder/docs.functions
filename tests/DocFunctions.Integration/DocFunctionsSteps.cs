﻿using System;
using TechTalk.SpecFlow;

namespace DocFunctions.Integration
{
    [Binding]
    public class DocFunctionsSteps
    {
        private string _newBlogName;

        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
            // Generate a new blog name based on the Current Date And Time

            // Validate that the blog does not exist direct url - should be 404

            // Validate that the blog does not exist on Cloudflared url - should be 404
            // Note this will also ensure that the Cloudflare cache has to be cleared (I think)
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I publish a new blog to my Github repo")]
        public void WhenIPublishANewBlogToMyGithubRepo()
        {
            // Create new blog (meta & md), with image

            // Commit to Github repo

            // Push Github repo (this should trigger the sequence)

            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I would expect the blog to be available on my website")]
        public void ThenIWouldExpectTheBlogToBeAvailableOnMyWebsite()
        {
            // Need to allow the DocsFunctions time to run

            // Maybe I need to have some way of monitoring the logs?

            // Validate that teh blog does exist direct url

            // Validate that the blog does exist on Cloudflare url

            ScenarioContext.Current.Pending();
        }
    }
}

using DocFunctions.Integration.Helpers;
using System;
using System.Configuration;
using TechTalk.SpecFlow;
using Tests.Common.Helpers;

namespace DocFunctions.Integration
{
    [Binding]
    public class DocFunctionsSteps
    {
        [Given(@"I don't already have a blog with name of the current date and time")]
        public void GivenIDonTAlreadyHaveABlogWithNameOfTheCurrentDateAndTime()
        {
            // Generate a new blog name based on the Current Date And Time
            var username = ConfigurationManager.AppSettings["github-username"];
            if (username == null) username = System.Environment.GetEnvironmentVariable("github-username");
            var key = ConfigurationManager.AppSettings["github-key"];
            if (key == null) key = System.Environment.GetEnvironmentVariable("github-key");
            var repo = ConfigurationManager.AppSettings["github-repo"];
            if (repo == null) repo = System.Environment.GetEnvironmentVariable("github-repo");

            var github = new GitHub(username, key, repo);
            github.CreateTestBog();

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

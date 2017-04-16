using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Unit.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit
{
    public class WebhookActionBuilderTests
    {
        [Fact]
        public void AddedFileCreatesNewBlogAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object);
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData);

            // Assert
            actionBuilder.Verify(m => m.NewBlog(It.IsAny<string>()));
            actionBuilder.Verify(m => m.Build());
            // Assert that the method on ActionBuilder was called
        }

        [Fact(Skip = "Draft test")]
        public void BuildGeneratedActionList()
        {
            // Arrange
            // Moq ActionBuidler
            // Add a Build to Moq which returns list of ActionList
            // Create WebhookActionBuilder - passing in the ActionBuilder

            // Act
            // Call build

            // Assert
            // Assert actions have been created
        }
    }
}

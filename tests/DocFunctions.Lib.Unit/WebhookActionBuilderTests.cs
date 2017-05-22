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
        }

        [Fact]
        public void AddedImageCreatesNewImageAction()
        {
            // Arrange
            var actionBuilder = new Mock<IActionBuilder>();
            var sut = new WebhookActionBuilder(actionBuilder.Object);
            var webhookData = new WebhookDataBuilder().Build();

            // Act
            sut.Process(webhookData);

            // Assert
            actionBuilder.Verify(m => m.NewImage(It.IsAny<string>(), It.IsAny<string>()));
            actionBuilder.Verify(m => m.Build());
        }
    }
}

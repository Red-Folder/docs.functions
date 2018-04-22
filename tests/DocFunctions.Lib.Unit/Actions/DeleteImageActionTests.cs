using DocFunctions.Lib.Unit.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit.Actions
{
    public class DeleteImageActionTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullAdded()
        {
            // Arrange
            var builder = new DeleteImageActionBuilder(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullFtpsClient()
        {
            // Arrange
            var builder = new DeleteImageActionBuilder(new Models.Github.Removed());
            builder.SetFtpsClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesFtpDeleteForImages()
        {
            // Arrange
            var builder = new DeleteImageActionBuilder(new Models.Github.Removed { FullFilename = @"test folder/image.png" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockFtpsClient.Verify(m => m.Delete(It.Is<string>(x => x == "/test folder/image.png")));
        }
    }
}

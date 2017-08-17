using DocFunctions.Lib.Unit.Builders;
using DocFunctions.Lib.Wappers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit.Actions
{
    public class ModifyImageActionTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullAdded()
        {
            // Arrange
            var builder = new ModifyImageActionBuilder(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullFtpsClient()
        {
            // Arrange
            var builder = new ModifyImageActionBuilder(new Models.Github.Modified());
            builder.SetFtpsClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesFtpDeleteForImages()
        {
            // Arrange
            var builder = new ModifyImageActionBuilder(new Models.Github.Modified { FullFilename = @"test folder/image.png" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockFtpsClient.Verify(m => m.Delete(It.Is<string>(x => x == "/site/mediaroot/blog/test folder/image.png")));
        }

        [Fact]
        public void ExecutesFtpUpdateForImages()
        {
            // Arrange
            var builder = new ModifyImageActionBuilder(new Models.Github.Modified { FullFilename = @"test folder/image.png" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockFtpsClient.Verify(m => m.Upload(It.Is<string>(x => x == "/site/mediaroot/blog/test folder/image.png"), It.IsAny<byte[]>()));
        }

        [Fact]
        public void ExecutesFTPInTheRightOrderForImages()
        {
            // Arrange
            var builder = new ModifyImageActionBuilder(new Models.Github.Modified { FullFilename = @"test folder/image.png" });

            var order = "";
            var mockFtp = new Mock<IFtpsClient>();
            mockFtp.Setup(x => x.Delete(It.IsAny<string>())).Callback(() => { order += "delete,"; });
            mockFtp.Setup(x => x.Upload(It.IsAny<string>(), It.IsAny<byte[]>())).Callback(() => { order += "upload"; });

            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            Assert.Equal("delete,upload", order);
        }


    }
}

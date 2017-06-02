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
    public class NewImageActionTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullPath()
        {
            // Arrange
            var builder = new NewImageActionBuilder(null, "image.png");
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullImageName()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullGithubReader()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            builder.SetGithubReader(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullFtpsClient()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            builder.SetFtpsClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            builder.SetBlogMetaProcessor(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesGithubGetForImages()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            // TODO
            //builder.MockGithubReader.Verify(m => m.GetRawImageFile(It.Is<string>(x => x == "/test folder/image.png")));
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogMeta()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            // TODO
            //builder.MockGithubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json")));
        }

        [Fact]
        public void ExecutesBlogMetaReaderOnRawBlogMeta()
        {
            // Arrange
            var builder = new NewImageActionBuilder("/test folder", "image.png");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaReader.Verify(m => m.Transform(It.Is<string>(x => x == "{}")));
        }

    }
}

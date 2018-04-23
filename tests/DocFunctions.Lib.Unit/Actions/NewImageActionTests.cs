using DocFunctions.Lib.Unit.Builders;
using Moq;
using System;
using Xunit;

namespace DocFunctions.Lib.Unit.Actions
{
    public class NewImageActionTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullAdded()
        {
            // Arrange
            var builder = new NewImageActionBuilder(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullGithubReader()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added());
            builder.SetGithubReader(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlobClient()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added());
            builder.SetBlobClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added());
            builder.SetBlogMetaProcessor(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesGithubGetForImages()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added { FullFilename = @"/test folder/image.png", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockGithubReader.Verify(m => m.GetRawImageFile(It.Is<string>(x => x == "/test folder/image.png"), It.Is<string>(x => x == "commit-sha-xxxx")));
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogMeta()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added { FullFilename = @"/test folder/image.png", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockGithubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json"), It.Is<string>(x => x == "commit-sha-xxxx")));
        }

        [Fact]
        public void ExecutesBlogMetaReaderOnRawBlogMeta()
        {
            // Arrange
            var builder = new NewImageActionBuilder(new Models.Github.Added { FullFilename = @"/test folder/image.png", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaReader.Verify(m => m.Transform(It.Is<string>(x => x == "{}")));
        }

    }
}

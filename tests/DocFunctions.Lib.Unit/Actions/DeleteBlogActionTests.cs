﻿using DocFunctions.Lib.Unit.Builders;
using docsFunctions.Shared.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit.Actions
{
    public class DeleteBlogActionTests
    {

        [Fact]
        public void ConstructorThrowsErrorOnNullPath()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullGithubReader()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            builder.SetGithubReader(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogClient()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            builder.SetBlogClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            builder.SetBlogMetaProcessor(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaRepository()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            builder.SetBlogMetaRepository(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesDeletesMarkup()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlobClient.Verify(m => m.Delete(It.Is<string>(x => x == "testblog/testblog.html")));
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogMeta()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
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
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaReader.Verify(m => m.Transform(It.Is<string>(x => x == "{}")));
        }

        [Fact]
        public void ExecutesDeleteBlogMeta()
        {
            // Arrange
            var builder = new DeleteBlogActionBuilder(new Models.Github.Removed { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaRepository.Verify(m => m.Delete(It.IsAny<string>()));
        }

    }
}

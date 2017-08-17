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
    public class ModifyBlogActionTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullPath()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullGithubReader()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            builder.SetGithubReader(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullMarkdownProcessor()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            builder.SetMarkdownProcessor(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullFtpsClient()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            builder.SetFtpsClient(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            builder.SetBlogMetaProcessor(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaRepository()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            builder.SetBlogMetaRepository(null);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogText()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockGithubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md"), It.Is<string>(x => x == "commit-sha-xxxx")));
        }

        [Fact]
        public void ExecutesMarkdownConvertOnRawBlogText()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockMarkdownProcessor.Verify(m => m.Process(It.Is<string>(x => x == "## Hello World")));
        }

        [Fact]
        public void ExecutesUploadsMarkup()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockFtpsClient.Verify(m => m.Upload(It.Is<string>(x => x == "/site/contentroot/testblog.html"), It.Is<string>(x => x == "<h2>Hello World</h2>")));
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogMeta()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
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
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaReader.Verify(m => m.Transform(It.Is<string>(x => x == "{}")));
        }

        [Fact]
        public void ExecutesSaveBlogMeta()
        {
            // Arrange
            var builder = new ModifyBlogActionBuilder(new Models.Github.Modified { FullFilename = "/test folder/blog.md", CommitShaForRead = "commit-sha-xxxx-old", CommitSha = "commit-sha-xxxx-new" });
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaRepository.Verify(m => m.Save(It.IsAny<Blog>()));
        }

    }
}

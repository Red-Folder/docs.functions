using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Unit.Builders;
using DocFunctions.Lib.Wappers;
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
    public class NewBlogActionTests
    {
        [Fact]
        public void ExecutesGithubGetForRawBlogText()
        {
            // Arrange
            var builder = new NewBlogActionBuilder("/test folder");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockGithubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md")));
        }

        [Fact]
        public void ExecutesMarkdownConvertOnRawBlogText()
        {
            // Arrange
            var builder = new NewBlogActionBuilder("/test folder");
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
            var builder = new NewBlogActionBuilder("/test folder");
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
            var builder = new NewBlogActionBuilder("/test folder");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockGithubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json")));
        }

        [Fact]
        public void ExecutesBlogMetaReaderOnRawBlogMeta()
        {
            // Arrange
            var builder = new NewBlogActionBuilder("/test folder");
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
            var builder = new NewBlogActionBuilder("/test folder");
            var sut = builder.Build();

            // Act
            sut.Execute();

            // Assert
            builder.MockBlogMetaRepository.Verify(m => m.Save(It.IsAny<Blog>()));
        }

    }
}

using DocFunctions.Lib.Actions;
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
            var githubReader = new Mock<IGithubReader>();

            var sut = new NewBlogAction("/test folder",
                                        githubReader.Object);

            // Act
            sut.Execute();

            // Assert
            githubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md")));
        }

        [Fact]
        public void ExecutesMarkdownConvertOnRawBlogText()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md")))
                .Returns("## Hello World");
            var markdownProcessor = new Mock<IMarkdownProcessor>();

            var sut = new NewBlogAction("/test folder", 
                                        githubReader.Object, 
                                        markdownProcessor.Object);

            // Act
            sut.Execute();

            // Assert
            markdownProcessor.Verify(m => m.Process(It.Is<string>(x => x == "## Hello World")));
        }

        [Fact]
        public void ExecutesUploadsMarkup()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.md")))
                .Returns("## Hello World");
            var markdownProcessor = new Mock<IMarkdownProcessor>();
            markdownProcessor.Setup(m => m.Process(It.Is<string>(x => x == "## Hello World")))
                .Returns("<h2>Hello World</h2>");
            var ftpsClient = new Mock<IFtpsClient>();
            var blogMetaReader = new Mock<IBlogMetaProcessor>();
            blogMetaReader.Setup(m => m.Transform(It.IsAny<string>()))
                .Returns(new Blog { Url = "testblog" });

            var sut = new NewBlogAction("/test folder", 
                                        githubReader.Object, 
                                        markdownProcessor.Object,
                                        ftpsClient.Object,
                                        blogMetaReader.Object);

            // Act
            sut.Execute();

            // Assert
            ftpsClient.Verify(m => m.Upload(It.Is<string>(x => x == "/site/contentroot/testblog.html"), It.Is<string>(x => x == "<h2>Hello World</h2>")));
        }

        [Fact]
        public void ExecutesGithubGetForRawBlogMeta()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();

            var sut = new NewBlogAction("/test folder",
                                        githubReader.Object);

            // Act
            sut.Execute();

            // Assert
            githubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json")));
        }

        [Fact]
        public void ExecutesBlogMetaReaderOnRawBlogMeta()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json")))
                .Returns("{}");
            var blogMetaReader = new Mock<IBlogMetaProcessor>();

            var sut = new NewBlogAction("/test folder",
                                        githubReader.Object,
                                        null,
                                        null,
                                        blogMetaReader.Object);

            // Act
            sut.Execute();

            // Assert
            blogMetaReader.Verify(m => m.Transform(It.Is<string>(x => x == "{}")));
        }

        [Fact]
        public void ExecutesSaveBlogMeta()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/blog.json")))
                .Returns("{}");
            var blogMetaReader = new Mock<IBlogMetaProcessor>();
            blogMetaReader.Setup(m => m.Transform(It.IsAny<string>()))
                .Returns(new Blog { Url = "testblog" });
            var blogMetaRepository = new Mock<IBlogMetaRepository>();

            var sut = new NewBlogAction("/test folder",
                                        githubReader.Object,
                                        null,
                                        null,
                                        blogMetaReader.Object,
                                        blogMetaRepository.Object);

            // Act
            sut.Execute();

            // Assert
            blogMetaRepository.Verify(m => m.Save(It.IsAny<Blog>()));
        }

    }
}

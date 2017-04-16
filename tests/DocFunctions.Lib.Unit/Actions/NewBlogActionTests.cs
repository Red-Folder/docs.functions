using DocFunctions.Lib.Actions;
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
            githubReader.Verify(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/*.md")));
        }

        [Fact]
        public void ExecutesMarkdownConvertOnRawBlogText()
        {
            // Arrange
            var githubReader = new Mock<IGithubReader>();
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/*.md")))
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
            githubReader.Setup(m => m.GetRawFile(It.Is<string>(x => x == "/test folder/*.md")))
                .Returns("## Hello World");
            var markdownProcessor = new Mock<IMarkdownProcessor>();
            markdownProcessor.Setup(m => m.Process(It.Is<string>(x => x == "## Hello World")))
                .Returns("<h2>Hello World</h2>");
            var ftpsClient = new Mock<IFtpsClient>();

            var sut = new NewBlogAction("/test folder", 
                                        githubReader.Object, 
                                        markdownProcessor.Object,
                                        ftpsClient.Object);

            // Act
            sut.Execute();

            // Assert
            ftpsClient.Verify(m => m.Upload(It.Is<string>(x => x == "/blogLocation/testblog.html"), It.Is<string>(x => x == "<h2>Hello World</h2>")));
        }

    }
}

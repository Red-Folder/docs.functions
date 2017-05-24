using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Wappers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit
{
    public class ActionBuilderTests
    {
        [Fact]
        public void ConstructorThrowsErrorOnNullGithubReader()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(null,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IFtpsClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullMarkdownProcessor()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            null,
                                                                            new Mock<IFtpsClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullFtpsClient()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            null,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IFtpsClient>().Object,
                                                                            null,
                                                                            new Mock<IBlogMetaRepository>().Object));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IFtpsClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            null));
        }


        [Fact]
        public void CreateSingleNewBlogAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IFtpsClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object);

            // Act
            sut.NewBlog("NewBlogPath");
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(NewBlogAction), actionList[0]);
        }

        [Fact]
        public void CreateSingleNewImageAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IFtpsClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object);

            // Act
            sut.NewImage("NewImagePath", "NewImage.png");
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(NewImageAction), actionList[0]);
        }
    }
}

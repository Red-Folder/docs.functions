using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Builders;
using DocFunctions.Lib.Models.Audit;
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
                                                                            new Mock<IBlobClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            new Mock<IWebCache>().Object,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullMarkdownProcessor()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            null,
                                                                            new Mock<IBlobClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            new Mock<IWebCache>().Object,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlobClient()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            null,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            new Mock<IWebCache>().Object,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaReader()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IBlobClient>().Object,
                                                                            null,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            new Mock<IWebCache>().Object,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullBlogMetaRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IBlobClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            null,
                                                                            new Mock<IWebCache>().Object,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullWebCache()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IBlobClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            null,
                                                                            new AuditTree("")));
        }

        [Fact]
        public void ConstructorThrowsErrorOnNullAuditTree()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionBuilder(new Mock<IGithubReader>().Object,
                                                                            new Mock<IMarkdownProcessor>().Object,
                                                                            new Mock<IBlobClient>().Object,
                                                                            new Mock<IBlogMetaProcessor>().Object,
                                                                            new Mock<IBlogMetaRepository>().Object,
                                                                            new Mock<IWebCache>().Object,
                                                                            null));
        }


        [Fact]
        public void CreateSingleNewBlogAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.NewBlog(new Models.Github.Added { FullFilename = @"Path/NewBlog.md" });
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
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.NewImage(new Models.Github.Added { FullFilename = @"Path/NewImage.png" });
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(NewImageAction), actionList[0]);
        }

        [Fact]
        public void CreateSingleDeleteBlogAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.DeleteBlog(new Models.Github.Removed { FullFilename = @"Path/DeletedBlog.md" } );
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(DeleteBlogAction), actionList[0]);
        }

        [Fact]
        public void CreateSingleDeleteImageAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.DeleteImage(new Models.Github.Removed { FullFilename = @"Path/DeletedImage.png" } );
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(DeleteImageAction), actionList[0]);
        }

        [Fact]
        public void CreateSingleModifiyBlogAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.ModifyBlog(new Models.Github.Modified { FullFilename = @"Path/DeletedBlog.md" });
            var actionList = sut.Build();

            // Assert
            Assert.Equal(2, actionList.Length);
            Assert.IsType(typeof(DeleteBlogAction), actionList[0]);
            Assert.IsType(typeof(NewBlogAction), actionList[1]);
        }

        [Fact]
        public void CreateSingleModifyImageAction()
        {
            // Arrange
            var sut = new ActionBuilder(new Mock<IGithubReader>().Object,
                                        new Mock<IMarkdownProcessor>().Object,
                                        new Mock<IBlobClient>().Object,
                                        new Mock<IBlogMetaProcessor>().Object,
                                        new Mock<IBlogMetaRepository>().Object,
                                        new Mock<IWebCache>().Object,
                                        new AuditTree(""));

            // Act
            sut.ModifyImage(new Models.Github.Modified { FullFilename = @"Path/DeletedImage.png" });
            var actionList = sut.Build();

            // Assert
            Assert.Equal(2, actionList.Length);
            Assert.IsType(typeof(DeleteImageAction), actionList[0]);
            Assert.IsType(typeof(NewImageAction), actionList[1]);
        }
    }
}

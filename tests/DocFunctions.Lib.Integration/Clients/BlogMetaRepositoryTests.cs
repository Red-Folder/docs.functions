using DocFunctions.Lib.Clients;
using docsFunctions.Shared.Models;
using System;
using System.Configuration;
using Xunit;

namespace DocFunctions.Lib.Integration.Clients
{
    public class BlogMetaRepositoryTests
    {
        [Fact]
        [Trait("Category","Integration")]
        public void SavesBlogs()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var containerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var sut = new BlogMetaRepository(connectionString, containerName);
            var newBlog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "Test blog meta used for integration test of the docFunctions"
            };

            // Act
            sut.Save(newBlog);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetAllBlogs()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var containerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var sut = new BlogMetaRepository(connectionString, containerName);

            // Act
            var result = sut.Get();

            // Assert
            Assert.True(result.HasValue);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetExisting()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var containerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var sut = new BlogMetaRepository(connectionString, containerName);
            var url = "2017-04-24-21-43-59";

            // Act
            var result = sut.Get(url);

            // Assert
            Assert.Equal(url, result.Value.Url);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetNotExistingReturnsNull()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["BlogMetaStorage"].ConnectionString;
            var containerName = ConfigurationManager.AppSettings["BlogMetaStorageContainerName"];
            var sut = new BlogMetaRepository(connectionString, containerName);
            var url = "I-DONT-EXIST";

            // Act
            var result = sut.Get(url);

            // Assert
            Assert.False(result.HasValue);
        }
    }
}

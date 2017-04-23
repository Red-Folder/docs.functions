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
    }
}

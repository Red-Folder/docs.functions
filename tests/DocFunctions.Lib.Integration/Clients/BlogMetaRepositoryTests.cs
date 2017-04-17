using DocFunctions.Lib.Clients;
using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Helpers;
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
            var connectionString = AppSettings.AppSetting("BlogMetaStorageConnectionString");
            var containerName = AppSettings.AppSetting("BlogMetaStorageContainerName");
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

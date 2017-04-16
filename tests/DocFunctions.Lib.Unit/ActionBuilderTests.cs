using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Builders;
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
        public void CreateSingleNewBlogAction()
        {
            // Arrange
            var sut = new ActionBuilder();

            // Act
            sut.NewBlog("NewBlogPath");
            var actionList = sut.Build();

            // Assert
            Assert.Equal(1, actionList.Length);
            Assert.IsType(typeof(NewBlogAction), actionList[0]);
        }
    }
}

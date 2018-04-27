using DocFunctions.Markdown;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using Xunit;
using docsFunctions.Shared.Models;
using DocFunctions.Markdown.Transformers;

namespace DocFunctions.Markdown.Unit
{
    public class MarkdownTransformerTests
    {
        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Empty_Inner()
        {
            
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "Hello World\n-----------\nText\n";

            // Using default contructor will create inner - passing null in will ensure no inner
            var uat = new MarkdownTransformer(null);

            var result = uat.Transform(meta, markdown);

            Assert.Contains(markdown, result);
        }

        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Inner()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "Hello World\n-----------\nText\n";

            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<Blog>(), It.IsAny<string>())).Returns("ABCDEF");

            var uat = new MarkdownTransformer(mock.Object);

            var result = uat.Transform(meta, markdown);

            Assert.NotNull(result);
            Assert.Contains("ABCDEF", result);
        }
    }
}

using DocFunctions.Lib.Processors;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using docsFunctions.Shared.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors
{
    public class MarkdownProcessorTests
    {
        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Empty_Inner()
        {
            var meta = new Blog();
                //JObject.Parse(
                //        @"{
                //            ""url"": ""/rfc-weekly-17th-October-2016"",
                //            ""published"": ""2016-10-17"",
                //            ""modified"": ""2016-10-17"",
                //            ""title"": ""RFC Weekly - 17th October 2016"",
                //            ""enabled"":  ""true""
                //        }");
            var markdown = "Hello World\n-----------\nText\n";

            // Using default contructor will create inner - passing null in will ensure no inner
            var uat = new MarkdownProcessor(null);

            var result = uat.Process(meta, markdown);

            Assert.NotNull(result);

            Assert.Contains(markdown, result);
        }

        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Inner()
        {
            var meta = new Blog();
                //JObject.Parse(
                //        @"{
                //            ""url"": ""/rfc-weekly-17th-October-2016"",
                //            ""published"": ""2016-10-17"",
                //            ""modified"": ""2016-10-17"",
                //            ""title"": ""RFC Weekly - 17th October 2016"",
                //            ""enabled"":  ""true""
                //        }");
            var markdown = "Hello World\n-----------\nText\n";

            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<Blog>(), It.IsAny<string>())).Returns("ABCDEF");

            var uat = new MarkdownProcessor(mock.Object);

            var result = uat.Process(meta, markdown);

            Assert.NotNull(result);
            Assert.Contains("ABCDEF", result);
        }

    }
}

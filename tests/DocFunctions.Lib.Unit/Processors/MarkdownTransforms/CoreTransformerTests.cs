using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using docsFunctions.Shared.Models;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class CoreTransformerTests
    {
        [Fact]
        public void Correctly_Convert_Markdown()
        {
            var meta = new Blog();
            //    JObject.Parse(
            //@"{
            //                ""url"": ""/rfc-weekly-17th-October-2016"",
            //                ""published"": ""2016-10-17"",
            //                ""modified"": ""2016-10-17"",
            //                ""title"": ""RFC Weekly - 17th October 2016"",
            //                ""enabled"":  ""true""
            //            }");
            var markdown = "Hello World\n-----------\nText\n";

            var uat = new CoreTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("<h2>Hello World</h2>", result);
            Assert.Contains("<p>Text</p>", result);
        }
    }
}

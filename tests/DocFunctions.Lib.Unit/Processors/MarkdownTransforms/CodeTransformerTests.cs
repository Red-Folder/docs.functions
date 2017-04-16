using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using docsFunctions.Shared.Models;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class CodeTrasnformerTests
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
            var markdown = "<p>Line1\n<code>Line2\nLine3\n</code>\nLine4</p>";
            var expected = "<p>Line1\n<pre><code>Line2\nLine3\n</code></pre>\nLine4</p>";

            var uat = new CodeTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }
    }
}

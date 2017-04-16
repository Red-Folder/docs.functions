using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class CoreTransformerTests
    {
        [Fact]
        public void Correctly_Convert_Markdown()
        {
            var markdown = "Hello World\n-----------\nText\n";

            var uat = new CoreTransformer();

            var result = uat.TransformMarkdown(markdown);

            Assert.Contains("<h2>Hello World</h2>", result);
            Assert.Contains("<p>Text</p>", result);
        }
    }
}

using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class CoreTransformerTests
    {
        [Fact]
        public void CorrectlyConvertMarkdown()
        {
            var markdown = "Hello World\n-----------\nText\n";

            var sut = new CoreTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains("<h2>Hello World</h2>", result);
            Assert.Contains("<p>Text</p>", result);
        }
    }
}

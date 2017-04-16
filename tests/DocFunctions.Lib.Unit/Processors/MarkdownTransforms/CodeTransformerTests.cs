using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class CodeTrasnformerTests
    {
        [Fact]
        public void Correctly_Convert_Markdown()
        {
            var markdown = "<p>Line1\n<code>Line2\nLine3\n</code>\nLine4</p>";
            var expected = "<p>Line1\n<pre><code>Line2\nLine3\n</code></pre>\nLine4</p>";

            var uat = new CodeTransformer();

            var result = uat.TransformMarkdown(markdown);

            Assert.Equal(expected, result);
        }
    }
}

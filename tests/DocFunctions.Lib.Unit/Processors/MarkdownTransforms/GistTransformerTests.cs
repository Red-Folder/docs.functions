using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class GistTransformerTests
    {
        [Fact]
        public void CorrectlyConvertMarkdown()
        {
            var markdown = "<p>Line1\n%[https://gist.github.com/3778380.js]\nLine4</p>";
            var expected = "<p>Line1\n<script src=\"https://gist.github.com/3778380.js\"></script>\nLine4</p>";

            var sut = new GistTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Equal(expected, result);
        }
    }
}

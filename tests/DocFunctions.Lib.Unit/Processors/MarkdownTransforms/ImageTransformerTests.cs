using DocFunctions.Lib.Processors.MarkdownTransforms;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class ImageTransformerTests
    {
        [Fact]
        public void CorrectlyAddTagToImageWithNoClass()
        {
            var markdown = "<img \\>";

            var sut = new ImageTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void CorrectlyAddTagToImageWithClassButNoResponsive()
        {
            var markdown = "<img \\>";

            var sut = new ImageTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void CorrectlyIgnoreImageWithClassWithResponsive()
        {
            var markdown = "<img class=\"img-responsive\"\\>";

            var sut = new ImageTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains(markdown, result);
        }
    }
}

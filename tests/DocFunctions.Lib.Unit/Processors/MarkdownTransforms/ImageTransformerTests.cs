using DocFunctions.Lib.Processors.MarkdownTransforms;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class ImageTransformerTests
    {
        [Fact]
        public void Correctly_Add_Tag_To_Image_With_No_Class()
        {
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Add_Tag_To_Image_With_Class_But_No_Responsive()
        {
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Ignore_Image_With_Class_With_Responsive()
        {
            var markdown = "<img class=\"img-responsive\"\\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(markdown);

            Assert.Contains(markdown, result);
        }
    }
}

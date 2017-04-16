using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class AcclaimTransformerTests
    {
        [Fact]
        public void CorrectlyConvertAPPBUILDER()
        {
            var markdown = "{ACCLAIM-APPBUILDER}";

            var sut = new AcclaimTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains("f57a78d2-31ae-42d5-a39d-48eaa1bd06cd", result);
        }

        [Fact]
        public void CorrectlyConvertWEBAPPLICATIONS()
        {
            var markdown = "{ACCLAIM-WEBAPPLICATIONS}";

            var sut = new AcclaimTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains("9ffb89e1-1f12-4382-9033-028aaebe793b", result);
        }

        [Fact]
        public void CorrectlyNotChanegIfInvalidACCLAIMTAG()
        {
            var markdown = "{ACCLAIM-NOTEXIST}";

            var sut = new AcclaimTransformer();

            var result = sut.TransformMarkdown(markdown);

            Assert.Contains(markdown, result);
        }
    }
}

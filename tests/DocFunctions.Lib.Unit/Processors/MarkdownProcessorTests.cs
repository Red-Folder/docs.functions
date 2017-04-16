using DocFunctions.Lib.Processors;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using Moq;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors
{
    public class MarkdownProcessorTests
    {
        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Empty_Inner()
        {
            var markdown = "Hello World\n-----------\nText\n";

            // Using default contructor will create inner - passing null in will ensure no inner
            var uat = new MarkdownProcessor(null);

            var result = uat.Process(markdown);

            Assert.NotNull(result);

            Assert.Contains(markdown, result);
        }

        [Fact]
        public void Blog_From_Valid_Meta_And_Markdown_With_Inner()
        {
            var markdown = "Hello World\n-----------\nText\n";

            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<string>())).Returns("ABCDEF");

            var uat = new MarkdownProcessor(mock.Object);

            var result = uat.Process(markdown);

            Assert.NotNull(result);
            Assert.Contains("ABCDEF", result);
        }

    }
}

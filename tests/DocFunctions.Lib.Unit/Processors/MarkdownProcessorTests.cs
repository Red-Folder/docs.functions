using DocFunctions.Lib.Processors;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using Moq;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors
{
    public class MarkdownProcessorTests
    {
        [Fact]
        public void BlogFromValidMetaAndMarkdownWithEmptyInner()
        {
            var markdown = "Hello World\n-----------\nText\n";

            // Using default contructor will create inner - passing null in will ensure no inner
            var sut = new MarkdownProcessor(null);

            var result = sut.Process(markdown);

            Assert.NotNull(result);

            Assert.Contains(markdown, result);
        }

        [Fact]
        public void BlogFromValidMetaAndMarkdownWithInner()
        {
            var markdown = "Hello World\n-----------\nText\n";

            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<string>())).Returns("ABCDEF");

            var sut = new MarkdownProcessor(mock.Object);

            var result = sut.Process(markdown);

            Assert.NotNull(result);
            Assert.Contains("ABCDEF", result);
        }

    }
}

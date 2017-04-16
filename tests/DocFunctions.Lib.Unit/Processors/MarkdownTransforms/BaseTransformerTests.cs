using Xunit;
using Moq;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class BaseTransformerTests
    {
        [Fact]
        public void NoChangeWithNoInner()
        {
            var sut = new BaseTransformer();

            var result = sut.TransformMarkdown("1234567890");

            Assert.Equal("1234567890", result);
        }

        [Fact]
        public void ChangeWithInnerPre()
        {
            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<string>())).Returns("ABCDEF");

            var sut = new BaseTransformer(mock.Object);

            var result = sut.TransformMarkdown("1234567890");

            Assert.Equal("ABCDEF", result);
        }
    }
}

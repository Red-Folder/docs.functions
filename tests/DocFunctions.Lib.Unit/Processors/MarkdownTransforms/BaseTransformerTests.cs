using Xunit;
using Moq;
using DocFunctions.Lib.Processors.MarkdownTransforms;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class BaseTransformerTests
    {
        [Fact]
        public void No_Change_With_No_Inner()
        {
            var uat = new BaseTransformer();

            var result = uat.TransformMarkdown("1234567890");

            Assert.Equal("1234567890", result);
        }

        [Fact]
        public void Change_With_Inner_Pre()
        {
            Mock<ITransformer> mock = new Mock<ITransformer>();
            mock.Setup(m => m.TransformMarkdown(It.IsAny<string>())).Returns("ABCDEF");

            var uat = new BaseTransformer(mock.Object);

            var result = uat.TransformMarkdown("1234567890");

            Assert.Equal("ABCDEF", result);
        }
    }
}

using DocFunctions.Lib.Processors;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors
{
    public class BlogMetaProcessorTests
    {
        [Fact]
        public void BlogFromValidMetaAndMarkdownWithEmptyInner()
        {
            var meta = @"{
                            ""url"": ""/rfc-weekly-17th-October-2016"",
                            ""published"": ""2016-10-17"",
                            ""modified"": ""2016-10-17"",
                            ""title"": ""RFC Weekly - 17th October 2016"",
                            ""enabled"":  ""true""
                        }";

            var sut = new BlogMetaProcessor();

            var result = sut.Transform(meta);

            Assert.NotNull(result);

            Assert.Equal(@"/rfc-weekly-17th-October-2016", result.Url);
            Assert.Equal(new DateTime(2016, 10, 17), result.Published);
            Assert.Equal(new DateTime(2016, 10, 17), result.Modified);
            Assert.Equal("RFC Weekly - 17th October 2016", result.Title);
            Assert.Equal(true, result.Enabled);
        }

    }
}

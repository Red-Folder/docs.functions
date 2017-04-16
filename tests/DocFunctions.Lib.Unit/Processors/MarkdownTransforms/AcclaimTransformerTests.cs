using Xunit;
using DocFunctions.Lib.Processors.MarkdownTransforms;
using docsFunctions.Shared.Models;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class AcclaimTransformerTests
    {
        [Fact]
        public void Correctly_Convert_APPBUILDER()
        {
            var meta = new Blog();
            //{
            //    url = "/rfc-weekly-17th-October-2016",
            //    published = ""2016 - 10 - 17"",
            //                ""modified"": ""2016 - 10 - 17"",
            //                ""title"": ""RFC Weekly -17th October 2016"",
            //                ""enabled"":  ""true""

            //};
            //    JObject.Parse(
            //@"{
            //            }");
            var markdown = "{ACCLAIM-APPBUILDER}";

            var uat = new AcclaimTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("f57a78d2-31ae-42d5-a39d-48eaa1bd06cd", result);
        }

        [Fact]
        public void Correctly_Convert_WEBAPPLICATIONS()
        {
            var meta = new Blog();
            //JObject.Parse(
            //@"{
            //                ""url"": ""/rfc-weekly-17th-October-2016"",
            //                ""published"": ""2016-10-17"",
            //                ""modified"": ""2016-10-17"",
            //                ""title"": ""RFC Weekly - 17th October 2016"",
            //                ""enabled"":  ""true""
            //            }");
            var markdown = "{ACCLAIM-WEBAPPLICATIONS}";

            var uat = new AcclaimTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("9ffb89e1-1f12-4382-9033-028aaebe793b", result);
        }

        [Fact]
        public void Correctly_Not_Chaneg_If_Invalid_ACCLAIMTAG()
        {
            var meta = new Blog();
            //    JObject.Parse(
            //@"{
            //                ""url"": ""/rfc-weekly-17th-October-2016"",
            //                ""published"": ""2016-10-17"",
            //                ""modified"": ""2016-10-17"",
            //                ""title"": ""RFC Weekly - 17th October 2016"",
            //                ""enabled"":  ""true""
            //            }");
            var markdown = "{ACCLAIM-NOTEXIST}";

            var uat = new AcclaimTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains(markdown, result);
        }
    }
}

using DocFunctions.Lib.Processors.MarkdownTransforms;
using docsFunctions.Shared.Models;
using Xunit;

namespace DocFunctions.Lib.Unit.Processors.MarkdownTransforms
{
    public class ImageTransformerTests
    {
        [Fact]
        public void Correctly_Add_Tag_To_Image_With_No_Class()
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
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Add_Tag_To_Image_With_Class_But_No_Responsive()
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
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Ignore_Image_With_Class_With_Responsive()
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
            var markdown = "<img class=\"img-responsive\"\\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains(markdown, result);
        }
    }
}

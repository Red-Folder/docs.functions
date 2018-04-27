using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class ImageTransformerTests
    {
        [Fact]
        public void Correctly_Add_Tag_To_Image_With_No_Class()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Add_Tag_To_Image_With_Class_But_No_Responsive()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<img \\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("class=\"img-responsive", result);
        }

        [Fact]
        public void Correctly_Ignore_Image_With_Class_With_Responsive()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<img class=\"img-responsive\"\\>";

            var uat = new ImageTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains(markdown, result);
        }
    }
}

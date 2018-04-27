using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class CoreTransformerTests
    {
        [Fact]
        public void Correctly_Convert_Markdown()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "Hello World\n-----------\nText\n";

            var uat = new CoreTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("<h2>Hello World</h2>", result);
            Assert.Contains("<p>Text</p>", result);
        }
    }
}

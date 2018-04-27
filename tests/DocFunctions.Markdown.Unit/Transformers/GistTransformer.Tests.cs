using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class GistTransformerTests
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
            var markdown = "<p>Line1\n%[https://gist.github.com/3778380.js]\nLine4</p>";
            var expected = "<p>Line1\n<script src=\"https://gist.github.com/3778380.js\"></script>\nLine4</p>";

            var uat = new GistTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }
    }
}

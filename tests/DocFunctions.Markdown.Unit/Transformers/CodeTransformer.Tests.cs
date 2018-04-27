using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class CodeTrasnformerTests
    {
        [Fact]
        public void Correctly_Convert_Code_Tag()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<p>Line1\n<code>Line2\nLine3\n</code>\nLine4</p>";
            var expected = "<p>Line1\n<pre><code>Line2\nLine3\n</code></pre>\nLine4</p>";

            var uat = new CodeTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Correctly_Convert_Tripple_Tick()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<p>Line1\n```\nLine2\nLine3\n```\nLine4</p>";
            var expected = "<p>Line1\n<pre><code>Line2\nLine3\n</code></pre>\nLine4</p>";

            var uat = new CodeTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Doesnt_Interpret_Comments_In_Code_As_Markdown()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "Line1\n# Line2\n```\n# Line3\n```\nLine4\n";
            var expected = "<p>Line1</p>\n\n<h1>Line2</h1>\n\n<pre><code># Line3\n</code></pre>\n\n<p>Line4</p>";

            // Needs to have the Core Transformer to validate the test
            var uat = new CodeTransformer(new CoreTransformer());

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }


        [Fact]
        public void Encode_Html_Elements_Within_Code_Blocks()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<div></div>\n```\n<div></div>\n```\n<div></div>\n";
            var expected = "<div></div>\n<pre><code>&lt;div&gt;&lt;/div&gt;\n</code></pre>\n<div></div>\n";

            var uat = new CodeTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(expected, result);
        }

    }
}

using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class ROIArticleTransformerTests
    {
        [Fact]
        public void MetaContainsNoKeywordThenNoROIBlock()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<h2>Hello World</h2>";

            var uat = new ROIArticleTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.DoesNotContain("roi-block", result);
            Assert.Equal(markdown, result);
        }

        [Fact]
        public void MetaContainsNoROIKeywordThenNoROIBlock()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = "<h2>Hello World</h2>";

            var uat = new ROIArticleTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.DoesNotContain("roi-block", result);
            Assert.Equal(markdown, result);
        }

        [Fact]
        public void MetaContainsROIKeywordThenROIBlock()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true,
                KeyWords = new List<string>
                {
                    "ROI"
                }
            };
            var markdown = "<h2>Hello World</h2>";

            var uat = new ROIArticleTransformer();

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Contains("roi-block", result);
            Assert.Contains(markdown, result);
        }
    }
}

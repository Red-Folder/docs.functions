using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Markdown.Unit.Transformers
{
    public class MediaUrlTransformerTests
    {
        [Fact]
        public void Vaidate_Tranform_Produces_Correct_Url()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = @"<img src='/media/blog/rfc-weekly-17th-October-2016/image.png' \><p>This bit should still ALLOW UPPERCASE<\p>";

            var uat = new MediaUrlTransformer("https://www.example.com/blog");

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(@"<img src='https://www.example.com/blog/rfc-weekly-17th-october-2016/image.png' \><p>This bit should still ALLOW UPPERCASE<\p>", result);
        }

        [Fact]
        public void Validate_Tranform_Produces_Correct_Url_For_Mutliple()
        {
            var meta = new Blog
            {
                Url = "/rfc-weekly-17th-October-2016",
                Published = DateTime.Now,
                Modified = DateTime.Now,
                Title = "RFC Weekly - 17th October 2016",
                Enabled = true
            };
            var markdown = @"<img src='/media/blog/rfc-weekly-17th-October-2016/image.png' \><img src='/media/blog/rfc-weekly-17th-October-2016/IMAGE2.png' \>";

            var uat = new MediaUrlTransformer("https://www.example.com/blog");

            var result = uat.TransformMarkdown(meta, markdown);

            Assert.Equal(@"<img src='https://www.example.com/blog/rfc-weekly-17th-october-2016/image.png' \><img src='https://www.example.com/blog/rfc-weekly-17th-october-2016/image2.png' \>", result);
        }

    }
}

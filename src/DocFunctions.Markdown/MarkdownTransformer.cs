using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DocFunctions.Markdown.Transformers;
using docsFunctions.Shared.Models;

namespace DocFunctions.Markdown
{
    public class MarkdownTransformer: IMarkdownTransformer
    {
        private ITransformer _innerTransformer;

        public MarkdownTransformer()
        {
            var core = new CoreTransformer();
            var code = new CodeTransformer(core);
            var acclaim = new AcclaimTransformer(code);
            var image = new ImageTransformer(acclaim);
            var gist = new GistTransformer(image);
            var roiArticle = new ROIArticleTransformer(gist);
            _innerTransformer = roiArticle;
        }

        public MarkdownTransformer(ITransformer innerTransformer)
        {
            _innerTransformer = innerTransformer;
        }

        public string Transform(Blog meta, string markdown)
        {
            if (_innerTransformer == null)
            {
                return markdown;
            }
            else
            {
                return _innerTransformer.TransformMarkdown(meta, markdown);
            }
        }
    }
}

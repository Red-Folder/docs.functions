using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public class BaseTransformer : ITransformer
    {
        ITransformer _innerTransformer;

        public BaseTransformer()
        {
        }
        public BaseTransformer(ITransformer innerTransformer)
        {
            _innerTransformer = innerTransformer;
        }

        public string TransformMarkdown(Blog meta, string markdown)
        {
            var partialTransform = PreTransform(meta, markdown);

            if (_innerTransformer != null)
            {
                partialTransform = _innerTransformer.TransformMarkdown(meta, partialTransform);
            }

            partialTransform = PostTransform(meta, partialTransform);

            return partialTransform;
        }

        protected virtual string PreTransform(Blog meta, string markdown)
        {
            return markdown;
        }

        protected virtual string PostTransform(Blog meta, string markdown)
        {
            return markdown;
        }
    }
}

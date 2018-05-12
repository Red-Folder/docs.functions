using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Markdown.Transformers
{
    public class MediaUrlTransformer : BaseTransformer
    {
        private string _baseUrl;

        public MediaUrlTransformer(string baseUrl) : base()
        {
            _baseUrl = baseUrl;
        }

        public MediaUrlTransformer(string baseUrl, ITransformer innerTransformer) : base(innerTransformer)
        {
            _baseUrl = baseUrl;
        }

        protected override string PostTransform(Blog meta, string markdown)
        {
            return markdown.Replace("/media/", _baseUrl + "/");
        }
    }
}
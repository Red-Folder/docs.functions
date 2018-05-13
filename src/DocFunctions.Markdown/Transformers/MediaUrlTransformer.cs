using docsFunctions.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocFunctions.Markdown.Transformers
{
    public class MediaUrlTransformer : BaseTransformer
    {
        private string _baseUrl;
        private Regex matchingPattern = new Regex("([\"']/media/blog/.*[\"'])");

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
            var newMarkdown = markdown;
            foreach (Match match in matchingPattern.Matches(markdown))
            {
                var originalMediaUrl = match.Value;
                var newMediaUrl = match.Value.Replace("/media/blog/", _baseUrl + "/").ToLower();
                newMarkdown = newMarkdown.Replace(originalMediaUrl, newMediaUrl);
            }
            //return markdown.Replace("/media/blog/", _baseUrl + "/").ToLower();
            return newMarkdown;
        }
    }
}
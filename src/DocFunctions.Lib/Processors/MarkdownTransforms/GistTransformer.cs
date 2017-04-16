using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public class GistTransformer : BaseTransformer
    {
        public GistTransformer() : base()
        {

        }

        public GistTransformer(ITransformer innerTransformer) : base(innerTransformer)
        {

        }

        protected override string PostTransform(Blog meta, string markdown)
        {
            return Regex.Replace(markdown, "%\\[(.*?)\\]", m => "<script src=\"" + m.Groups[1].Value + "\"></script>");
        }
    }
}

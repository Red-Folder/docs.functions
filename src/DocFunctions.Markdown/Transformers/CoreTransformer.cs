using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;

namespace DocFunctions.Markdown.Transformers
{
    public class CoreTransformer : BaseTransformer
    {
        protected override string PostTransform(Blog meta, string markdown)
        {
            HeyRed.MarkdownSharp.Markdown processor = new HeyRed.MarkdownSharp.Markdown();

            return processor.Transform(markdown);
        }
    }
}

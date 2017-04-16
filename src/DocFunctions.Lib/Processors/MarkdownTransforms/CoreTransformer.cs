using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using docsFunctions.Shared.Models;

namespace DocFunctions.Lib.Processors.MarkdownTransforms
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

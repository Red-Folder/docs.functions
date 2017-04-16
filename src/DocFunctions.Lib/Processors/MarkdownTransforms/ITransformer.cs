using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public interface ITransformer
    {
        string TransformMarkdown(Blog meta, string markdown);
    }
}

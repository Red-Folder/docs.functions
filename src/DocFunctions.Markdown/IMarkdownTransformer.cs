using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Markdown
{
    public interface IMarkdownTransformer
    {
        string Transform(Blog meta, string markdown);
    }
}

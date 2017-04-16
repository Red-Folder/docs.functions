using System.Text.RegularExpressions;

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

        protected override string PostTransform(string markdown)
        {
            return Regex.Replace(markdown, "%\\[(.*?)\\]", m => "<script src=\"" + m.Groups[1].Value + "\"></script>");
        }
    }
}

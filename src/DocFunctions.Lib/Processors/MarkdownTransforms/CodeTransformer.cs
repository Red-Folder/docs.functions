namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public class CodeTransformer: BaseTransformer
    {
        public CodeTransformer() : base()
        {

        }

        public CodeTransformer(ITransformer innerTransformer) : base(innerTransformer)
        {

        }

        protected override string PostTransform(string markdown)
        {
            //return Regex.Replace(markdown, "<code>(.*)</code>", m => m.Value.Replace("\n", "<br/>\n"), RegexOptions.Singleline);
            return markdown.Replace("<code>", "<pre><code>").Replace("</code>", "</code></pre>").Replace("<code>\n","<code>");
        }
    }
}

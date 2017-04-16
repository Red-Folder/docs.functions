namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public class CoreTransformer : BaseTransformer
    {
        protected override string PostTransform(string markdown)
        {
            HeyRed.MarkdownSharp.Markdown processor = new HeyRed.MarkdownSharp.Markdown();

            return processor.Transform(markdown);
        }
    }
}

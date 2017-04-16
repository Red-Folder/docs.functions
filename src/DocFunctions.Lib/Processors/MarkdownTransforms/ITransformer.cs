namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public interface ITransformer
    {
        string TransformMarkdown(string markdown);
    }
}

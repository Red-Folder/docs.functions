namespace DocFunctions.Lib.Processors.MarkdownTransforms
{
    public class BaseTransformer : ITransformer
    {
        ITransformer _innerTransformer;

        public BaseTransformer()
        {
        }
        public BaseTransformer(ITransformer innerTransformer)
        {
            _innerTransformer = innerTransformer;
        }

        public string TransformMarkdown(string markdown)
        {
            var partialTransform = PreTransform(markdown);

            if (_innerTransformer != null)
            {
                partialTransform = _innerTransformer.TransformMarkdown(partialTransform);
            }

            partialTransform = PostTransform(partialTransform);

            return partialTransform;
        }

        protected virtual string PreTransform(string markdown)
        {
            return markdown;
        }

        protected virtual string PostTransform(string markdown)
        {
            return markdown;
        }
    }
}

using DocFunctions.Lib.Processors.MarkdownTransforms;
using DocFunctions.Lib.Wappers;

namespace DocFunctions.Lib.Processors
{
    public class MarkdownProcessor : IMarkdownProcessor
    {
        private ITransformer _innerTransformer;

        public MarkdownProcessor()
        {
            var core = new CoreTransformer();
            var code = new CodeTransformer(core);
            var acclaim = new AcclaimTransformer(code);
            var image = new ImageTransformer(acclaim);
            var gist = new GistTransformer(image);
            _innerTransformer = gist;
        }

        public MarkdownProcessor(ITransformer innerTransformer)
        {
            _innerTransformer = innerTransformer;
        }

        public string Process(string markdown)
        {
            if (_innerTransformer == null)
            {
                return markdown;
            }
            else
            {
                return _innerTransformer.TransformMarkdown(markdown);
            }
        }
    }
}

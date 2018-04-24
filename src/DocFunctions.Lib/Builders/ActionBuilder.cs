using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Builders
{
    public class ActionBuilder : IActionBuilder
    {
        private IGithubReader _githubReader;
        private IMarkdownProcessor _markdownProcessor;
        private IBlobClient _blobClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;
        private IWebCache _cache;
        private AuditTree _audit;

        private List<IAction> _actions = new List<IAction>();

        public ActionBuilder(IGithubReader githubReader,
                             IMarkdownProcessor markdownProcessor,
                             IBlobClient blobClient,
                             IBlogMetaProcessor blogMetaReader,
                             IBlogMetaRepository blogMetaRepository,
                             IWebCache cache,
                             AuditTree audit)
        {
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _blobClient = blobClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
            _audit = audit;
        }

        public IAction[] Build()
        {
            return _actions.ToArray();
        }

        public IActionBuilder NewBlog(Added added)
        {
            _actions.Add(new NewBlogAction(added,
                                            _githubReader,
                                            _markdownProcessor,
                                            _blobClient,
                                            _blogMetaReader,
                                            _blogMetaRepository,
                                            _cache,
                                            _audit));

            return this;
        }

        public IActionBuilder NewImage(Added added)
        {
            _actions.Add(new NewImageAction(added,
                                                _githubReader,
                                                _blobClient,
                                                _blogMetaReader,
                                                _cache,
                                                _audit));
            return this;
        }

        public IActionBuilder Clear()
        {
            _actions.Clear();

            return this;
        }

        public IActionBuilder DeleteBlog(Removed removed)
        {
            _actions.Add(new DeleteBlogAction(removed,
                                            _githubReader,
                                            _blobClient,
                                            _blogMetaReader,
                                            _blogMetaRepository,
                                            _cache,
                                            _audit));

            return this;
        }

        public IActionBuilder DeleteImage(Removed removed)
        {
            _actions.Add(new DeleteImageAction(removed,
                                            _blobClient,
                                            _cache,
                                            _audit));

            return this;
        }

        public IActionBuilder ModifyBlog(Modified modified)
        {
            DeleteBlog(modified);
            NewBlog(modified);
            return this;
        }

        public IActionBuilder ModifyImage(Modified modified)
        {
            DeleteImage(modified);
            NewImage(modified);
            return this;
        }
    }
}

using DocFunctions.Lib.Actions;
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
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;

        private List<IAction> _actions = new List<IAction>();

        public ActionBuilder(IGithubReader githubReader,
                             IMarkdownProcessor markdownProcessor,
                             IFtpsClient ftpsClient,
                             IBlogMetaProcessor blogMetaReader,
                             IBlogMetaRepository blogMetaRepository)
        {
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");

            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
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
                                            _ftpsClient,
                                            _blogMetaReader,
                                            _blogMetaRepository));

            return this;
        }

        public IActionBuilder NewImage(Added added)
        {
            _actions.Add(new NewImageAction(added,
                                                _githubReader,
                                                _ftpsClient,
                                                _blogMetaReader));
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
                                            _ftpsClient,
                                            _blogMetaReader,
                                            _blogMetaRepository));

            return this;
        }

        public IActionBuilder DeleteImage(Removed removed)
        {
            _actions.Add(new DeleteImageAction(removed,
                                            _ftpsClient));

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

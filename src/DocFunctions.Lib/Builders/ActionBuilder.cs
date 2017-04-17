using DocFunctions.Lib.Actions;
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

        public ActionBuilder(IGithubReader githubReader = null,
                             IMarkdownProcessor markdownProcessor = null,
                             IFtpsClient ftpsClient = null,
                             IBlogMetaProcessor blogMetaReader = null,
                             IBlogMetaRepository blogMetaRepository = null)
        {
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

        public IActionBuilder NewBlog(string blogPath)
        {
            _actions.Add(new NewBlogAction(blogPath,
                                            _githubReader,
                                            _markdownProcessor,
                                            _ftpsClient,
                                            _blogMetaReader,
                                            _blogMetaRepository));

            return this;
        }
    }
}

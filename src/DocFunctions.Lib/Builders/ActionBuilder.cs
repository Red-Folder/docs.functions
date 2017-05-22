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
        private IFtpsClient _ftpsClientForHtml;
        private IFtpsClient _ftpsClientForImage;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;

        private List<IAction> _actions = new List<IAction>();

        public ActionBuilder(IGithubReader githubReader,
                             IMarkdownProcessor markdownProcessor,
                             IFtpsClient ftpsClientForHtml,
                             IFtpsClient ftpsClientForImage,
                             IBlogMetaProcessor blogMetaReader,
                             IBlogMetaRepository blogMetaRepository)
        {
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (ftpsClientForHtml == null) throw new ArgumentNullException("ftpsClientForHtml");
            if (ftpsClientForImage == null) throw new ArgumentNullException("ftpsClientForImage");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");

            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _ftpsClientForHtml = ftpsClientForHtml;
            _ftpsClientForImage = ftpsClientForImage;
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
                                            _ftpsClientForHtml,
                                            _blogMetaReader,
                                            _blogMetaRepository));

            return this;
        }

        public IActionBuilder NewImage(string blogPath, string image)
        {
            _actions.Add(new NewImageAction(blogPath,
                                                image,
                                                _githubReader,
                                                _ftpsClientForImage,
                                                _blogMetaReader));
            return this;
        }

        public IActionBuilder Clear()
        {
            _actions.Clear();

            return this;
        }
    }
}

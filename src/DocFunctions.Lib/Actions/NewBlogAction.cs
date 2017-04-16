using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Actions
{
    public class NewBlogAction : IAction
    {
        private string _blogPath;
        private IGithubReader _githubReader;
        private IMarkdownProcessor _markdownProcessor;

        public NewBlogAction(string blogPath,
                                IGithubReader githubReader = null,
                                IMarkdownProcessor markdownProcessor = null)
        {
            _blogPath = blogPath;
            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
        }

        public void Execute()
        {
            var blogMarkdown = GetMarkdownFromGithub();
            var blogMarkup = ConvertRawBlogToMarkup(blogMarkdown);
        }

        private string GetMarkdownFromGithub()
        {
            if (_githubReader == null) return "";

            return _githubReader.GetRawFile(_blogPath + "/*.md");
        }

        private string ConvertRawBlogToMarkup(string blogMarkdown)
        {
            if (_markdownProcessor == null) return "";

            return _markdownProcessor.Process(blogMarkdown);
        }
    }
}

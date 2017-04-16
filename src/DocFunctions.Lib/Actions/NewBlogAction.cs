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
        private IFtpsClient _ftpsClient;

        public NewBlogAction(string blogPath,
                                IGithubReader githubReader = null,
                                IMarkdownProcessor markdownProcessor = null,
                                IFtpsClient ftpsClient = null)
        {
            _blogPath = blogPath;
            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _ftpsClient = ftpsClient;
        }

        public void Execute()
        {
            var blogMarkdown = GetMarkdownFromGithub();
            var blogMarkup = ConvertRawBlogToMarkup(blogMarkdown);
            UploadBlogMarkup(blogMarkup);
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

        private void UploadBlogMarkup(string markup)
        {
            if (_ftpsClient == null) return;

            _ftpsClient.Upload("/blogLocation/testblog.html", markup);
        }
    }
}

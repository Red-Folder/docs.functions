using DocFunctions.Lib.Wappers;
using docsFunctions.Shared.Models;
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
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;


        public NewBlogAction(string blogPath,
                                IGithubReader githubReader = null,
                                IMarkdownProcessor markdownProcessor = null,
                                IFtpsClient ftpsClient = null,
                                IBlogMetaProcessor blogMetaReader = null,
                                IBlogMetaRepository blogMetaRepository = null)
        {
            _blogPath = blogPath;
            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
        }

        public void Execute()
        {
            var blogMetaJson = GetMetaJsonFromGithub();
            var blogMeta = GetMetaFromMetaJson(blogMetaJson);

            var blogMarkdown = GetMarkdownFromGithub();
            var blogMarkup = ConvertRawBlogToMarkup(blogMarkdown);
            UploadBlogMarkup(blogMeta, blogMarkup);

            SaveBlogMeta(blogMeta);
        }

        private string GetMetaJsonFromGithub()
        {
            if (_githubReader == null) return "";

            return _githubReader.GetRawFile(_blogPath + "/blog.json");
        }

        private Blog GetMetaFromMetaJson(string blogMetaJson)
        {
            if (_blogMetaReader == null) return null;

            return _blogMetaReader.Transform(blogMetaJson);
        }

        private void SaveBlogMeta(Blog blogMeta)
        {
            if (_blogMetaRepository == null) return;

            _blogMetaRepository.Save(blogMeta);
        }

        private string GetMarkdownFromGithub()
        {
            if (_githubReader == null) return "";

            return _githubReader.GetRawFile(_blogPath + "/blog.md");
        }

        private string ConvertRawBlogToMarkup(string blogMarkdown)
        {
            if (_markdownProcessor == null) return "";

            return _markdownProcessor.Process(blogMarkdown);
        }

        private void UploadBlogMarkup(Blog blogMeta, string markup)
        {
            if (_ftpsClient == null) return;

            _ftpsClient.Upload("/site/contentroot/" + blogMeta.Url + ".html", markup);
        }
    }
}

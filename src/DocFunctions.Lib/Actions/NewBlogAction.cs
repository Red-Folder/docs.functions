using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using docsFunctions.Shared.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Actions
{
    public class NewBlogAction : IAction
    {
        private Added _data;
        private IGithubReader _githubReader;
        private IMarkdownProcessor _markdownProcessor;
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;


        public NewBlogAction(Added data,
                                IGithubReader githubReader,
                                IMarkdownProcessor markdownProcessor,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");

            _data = data;
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
            return _githubReader.GetRawFile(_data.Path + "/blog.json", _data.CommitShaForRead);
        }

        private Blog GetMetaFromMetaJson(string blogMetaJson)
        {
            return _blogMetaReader.Transform(blogMetaJson);
        }

        private void SaveBlogMeta(Blog blogMeta)
        {
            Log.Information("Saving {Url} to meta repo", blogMeta.Url);
            _blogMetaRepository.Save(blogMeta);
        }

        private string GetMarkdownFromGithub()
        {
            return _githubReader.GetRawFile(_data.Path + "/blog.md", _data.CommitShaForRead);
        }

        private string ConvertRawBlogToMarkup(string blogMarkdown)
        {
            return _markdownProcessor.Process(blogMarkdown);
        }

        private void UploadBlogMarkup(Blog blogMeta, string markup)
        {
            var filename = "/site/contentroot/" + blogMeta.Url + ".html";
            Log.Information("Using Ftps to upload: {filename}", filename);
            _ftpsClient.Upload(filename, markup);
        }
    }
}

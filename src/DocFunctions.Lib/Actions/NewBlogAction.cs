using DocFunctions.Lib.Models.Audit;
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
        private IWebCache _cache;

        public NewBlogAction(Added data,
                                IGithubReader githubReader,
                                IMarkdownProcessor markdownProcessor,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository,
                                IWebCache cache)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");

            _data = data;
            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
        }

        public void Execute()
        {
            AuditTree.Instance.StartOperation($"Executing New Blog Action for {_data.Filename}");
            try
            {
                AuditTree.Instance.Add("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();
                AuditTree.Instance.Add("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                AuditTree.Instance.Add("Getting the Markdown from Github");
                var blogMarkdown = GetMarkdownFromGithub();
                AuditTree.Instance.Add("Converting the Markdown to HTML");
                var blogMarkup = ConvertRawBlogToMarkup(blogMarkdown);
                AuditTree.Instance.Add("Uploading the HTML to the server");
                UploadBlogMarkup(blogMeta, blogMarkup);

                AuditTree.Instance.Add("Saving to the Blog Meta to the repository");
                SaveBlogMeta(blogMeta);

                AuditTree.Instance.Add($"Removing cache for {blogMeta.Url}");
                _cache.RemoveCachedInstances(blogMeta.Url);
            }
            catch (Exception ex)
            {
                AuditTree.Instance.AddFailure($"Failed due to exception: {ex.Message}");
            }
            AuditTree.Instance.EndOperation();
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

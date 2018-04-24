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
        private IBlobClient _blobClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;
        private IWebCache _cache;
        private AuditTree _audit;

        public NewBlogAction(Added data,
                                IGithubReader githubReader,
                                IMarkdownProcessor markdownProcessor,
                                IBlobClient blobClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository,
                                IWebCache cache,
                                AuditTree audit)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownProcessor == null) throw new ArgumentNullException("markdownProcessor");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _data = data;
            _githubReader = githubReader;
            _markdownProcessor = markdownProcessor;
            _blobClient = blobClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
            _audit = audit;
        }

        public void Execute()
        {
            _audit.StartOperation($"Executing New Blog Action for {_data.Filename}");
            try
            {
                _audit.Add("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();
                _audit.Add("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                _audit.Add("Getting the Markdown from Github");
                var blogMarkdown = GetMarkdownFromGithub();
                _audit.Add("Converting the Markdown to HTML");
                var blogMarkup = ConvertRawBlogToMarkup(blogMarkdown);
                _audit.Add("Uploading the HTML to the server");
                UploadBlogMarkup(blogMeta, blogMarkup);

                _audit.Add("Saving to the Blog Meta to the repository");
                SaveBlogMeta(blogMeta);

                _audit.Add($"Removing cache for {blogMeta.Url}");
                _cache.RemoveCachedInstances(blogMeta.Url);
            }
            catch (Exception ex)
            {
                _audit.AddFailure($"Failed due to exception: {ex.Message}");
            }
            _audit.EndOperation();
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
            var filename = $"{blogMeta.Url}/{blogMeta.Url}.html";
            Log.Information("Uploading: {filename}", filename);
            _blobClient.Upload(filename, markup);
        }
    }
}

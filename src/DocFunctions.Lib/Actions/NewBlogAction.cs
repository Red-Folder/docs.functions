using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using DocFunctions.Markdown;
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
        private IMarkdownTransformer _markdownTransformer;
        private IBlobClient _blobClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;
        private IWebCache _cache;
        private AuditTree _audit;

        public NewBlogAction(Added data,
                                IGithubReader githubReader,
                                IMarkdownTransformer markdownTransformer,
                                IBlobClient blobClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository,
                                IWebCache cache,
                                AuditTree audit)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (markdownTransformer == null) throw new ArgumentNullException("markdownTrasnformer");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _data = data;
            _githubReader = githubReader;
            _markdownTransformer = markdownTransformer;
            _blobClient = blobClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
            _audit = audit;
        }

        public void Execute()
        {
            _audit.StartOperation($"Executing New Blog Action for {_data.Path}");
            try
            {
                _audit.Audit("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();

                _audit.Audit($"JSON = {blogMetaJson}");

                _audit.Audit("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                _audit.Audit("Getting the Markdown from Github");
                var blogMarkdown = GetMarkdownFromGithub();
                _audit.Audit("Converting the Markdown to HTML");
                var blogMarkup = ConvertRawBlogToMarkup(blogMeta, blogMarkdown);
                _audit.Audit("Uploading the HTML to the server");
                UploadBlogMarkup(blogMeta, blogMarkup);

                _audit.Audit("Saving to the Blog Meta to the repository");
                SaveBlogMeta(blogMeta);

                _audit.Audit($"Removing cache for {blogMeta.Url}");
                _cache.RemoveCachedInstances(blogMeta.Url);
            }
            catch (Exception ex)
            {
                _audit.Error(ex.Message, ex);
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

        private string ConvertRawBlogToMarkup(Blog blogMeta, string blogMarkdown)
        {
            return _markdownTransformer.Transform(blogMeta, blogMarkdown);
        }

        private void UploadBlogMarkup(Blog blogMeta, string markup)
        {
            var filename = blogMeta.ContentFile;
            Log.Information("Uploading: {filename}", filename);
            _blobClient.Upload(filename, markup);
        }
    }
}

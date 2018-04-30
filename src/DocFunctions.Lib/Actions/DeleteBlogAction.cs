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
    public class DeleteBlogAction : IAction
    {
        private Removed _data;
        private IGithubReader _githubReader;
        private IBlobClient _blobClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;
        private IWebCache _cache;
        private AuditTree _audit;

        public DeleteBlogAction(Removed data,
                                IGithubReader githubReader,
                                IBlobClient blobClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository,
                                IWebCache cache,
                                AuditTree audit)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _data = data;
            _githubReader = githubReader;
            _blobClient = blobClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
            _audit = audit;
        }

        public void Execute()
        {
            _audit.StartOperation($"Executing Delete Blog Action for {_data.Filename}");
            try
            {
                _audit.Audit("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();
                _audit.Audit("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                _audit.Audit("Deleting the HTML from the server");
                DeleteBlogMarkup(blogMeta);

                _audit.Audit("Deleting the Blog Meta from the respository");
                DeleteBlogMeta(blogMeta);

                _audit.Audit($"Removing cache for {blogMeta.Url}");
                _cache.RemoveCachedInstances(blogMeta.Url);
            }
            catch (Exception ex)
            {
                _audit.Error($"Failed due to exception: {ex.Message}", ex);
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

        private void DeleteBlogMeta(Blog blogMeta)
        {
            Log.Information("Removing {Url} from meta repo", blogMeta.Url);
            _blogMetaRepository.Delete(blogMeta.Url);
        }

        private void DeleteBlogMarkup(Blog blogMeta)
        {
            var filename = $"{blogMeta.Url}/{blogMeta.Url}.html";
            Log.Information("Deleting: {filename}", filename);
            _blobClient.Delete(filename);
        }
    }
}

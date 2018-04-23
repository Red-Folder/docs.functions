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
        private IBlobClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IBlogMetaRepository _blogMetaRepository;
        private IWebCache _cache;

        public DeleteBlogAction(Removed data,
                                IGithubReader githubReader,
                                IBlobClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader,
                                IBlogMetaRepository blogMetaRepository,
                                IWebCache cache)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (blogMetaRepository == null) throw new ArgumentNullException("blogMetaRepository");
            if (cache == null) throw new ArgumentNullException("cache");

            _data = data;
            _githubReader = githubReader;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _blogMetaRepository = blogMetaRepository;
            _cache = cache;
        }

        public void Execute()
        {
            AuditTree.Instance.StartOperation($"Executing Delete Blog Action for {_data.Filename}");
            try
            {
                AuditTree.Instance.Add("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();
                AuditTree.Instance.Add("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                AuditTree.Instance.Add("Deleting the HTML from the server");
                DeleteBlogMarkup(blogMeta);

                AuditTree.Instance.Add("Deleting the Blog Meta from the respository");
                DeleteBlogMeta(blogMeta);

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

        private void DeleteBlogMeta(Blog blogMeta)
        {
            Log.Information("Removing {Url} from meta repo", blogMeta.Url);
            _blogMetaRepository.Delete(blogMeta.Url);
        }

        private void DeleteBlogMarkup(Blog blogMeta)
        {
            var filename = $"{blogMeta.Url}/{blogMeta.Url}.html";
            Log.Information("Using Ftps to delete: {filename}", filename);
            _ftpsClient.Delete(filename);
        }
    }
}

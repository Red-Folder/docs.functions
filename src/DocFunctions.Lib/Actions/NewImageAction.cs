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
    public class NewImageAction : IAction
    {
        private Added _data;
        private IGithubReader _githubReader;
        private IFtpsClient _ftpsClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IWebCache _cache;

        public NewImageAction(Added data,
                                IGithubReader githubReader,
                                IFtpsClient ftpsClient,
                                IBlogMetaProcessor blogMetaReader,
                                IWebCache cache)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (cache == null) throw new ArgumentNullException("cache");

            _data = data;
            _githubReader = githubReader;
            _ftpsClient = ftpsClient;
            _blogMetaReader = blogMetaReader;
            _cache = cache;
        }

        public void Execute()
        {
            AuditTree.Instance.StartOperation($"Executing New Image Action for {_data.Filename}");
            try
            {
                AuditTree.Instance.Add("Getting Json from Github");
                var blogMetaJson = GetMetaJsonFromGithub();
                AuditTree.Instance.Add("Converting the Json to Blog Meta data");
                var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                AuditTree.Instance.Add("Getting Image from Github");
                var blogImage = GetImageFromGithub();
                AuditTree.Instance.Add("Uploading Image to the server");
                UploadImage(blogMeta, blogImage);

                AuditTree.Instance.Add($"Removing cache for TODO - need image url");
                _cache.RemoveCachedInstances("TODO - need image url");
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

        private byte[] GetImageFromGithub()
        {
            return _githubReader.GetRawImageFile($"{_data.FullFilename}", _data.CommitShaForRead);
        }

        private void UploadImage(Blog blogMeta, byte[] image)
        {
            var filename = $"/site/mediaroot/blog/{blogMeta.Url}/{_data.Filename}";
            Log.Information("Using Ftps to upload: {filename}", filename);
            _ftpsClient.Upload(filename, image);
        }
    }
}

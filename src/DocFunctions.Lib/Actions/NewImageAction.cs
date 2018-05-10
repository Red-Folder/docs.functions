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
using static DocFunctions.Lib.Clients.GithubClient;

namespace DocFunctions.Lib.Actions
{
    public class NewImageAction : IAction
    {
        private Added _data;
        private IGithubReader _githubReader;
        private IBlobClient _blobClient;
        private IBlogMetaProcessor _blogMetaReader;
        private IWebCache _cache;
        private AuditTree _audit;

        public NewImageAction(Added data,
                                IGithubReader githubReader,
                                IBlobClient blobClient,
                                IBlogMetaProcessor blogMetaReader,
                                IWebCache cache,
                                AuditTree audit)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (githubReader == null) throw new ArgumentNullException("githubReader");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (blogMetaReader == null) throw new ArgumentNullException("blogMetaReader");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _data = data;
            _githubReader = githubReader;
            _blobClient = blobClient;
            _blogMetaReader = blogMetaReader;
            _cache = cache;
            _audit = audit;
        }

        public void Execute()
        {
            _audit.StartOperation($"Executing New Image Action for {_data.Path}: {_data.Filename}");
            try
            {
                var destinationPath = "";
                try
                {
                    _audit.Audit("Getting Json from Github");
                    var blogMetaJson = GetMetaJsonFromGithub();
                    _audit.Audit("Converting the Json to Blog Meta data");
                    var blogMeta = GetMetaFromMetaJson(blogMetaJson);

                    destinationPath = blogMeta.Url;
                } catch (Exception ex)
                {
                    // If just an image folder rather than full blog, then just use the _data.path as the destination
                    destinationPath = _data.Path;
                }
                _audit.Audit("Getting Image from Github");
                var blogImage = GetImageFromGithub();
                _audit.Audit("Uploading Image to the server");
                UploadImage(destinationPath, blogImage);

                _audit.Audit($"Removing cache for TODO - need image url");
                _cache.RemoveCachedInstances("TODO - need image url");
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

        private byte[] GetImageFromGithub()
        {
            return _githubReader.GetRawImageFile($"{_data.FullFilename}", _data.CommitShaForRead);
        }

        private void UploadImage(string destinationPath, byte[] image)
        {
            var filename = $"{destinationPath}/{_data.Filename}";
            Log.Information("Uploading: {filename}", filename);
            _blobClient.Upload(filename, image);
        }
    }
}

using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using docsFunctions.Shared.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Microsoft.Azure;
using Functional.Maybe;
using System.Diagnostics.CodeAnalysis;

namespace DocFunctions.Lib.Clients
{
    [ExcludeFromCodeCoverage]
    public class BlogMetaRepository : IBlogMetaRepository
    {
        private const string BLOGMETAFILENAME = "BlogMeta.json";

        private List<Blog> _blogs;

        private CloudBlobContainer _container = null;

        public BlogMetaRepository(string connectionString, string containerName)
        {
            InitialiseContainer(connectionString, containerName);
        }

        private void InitialiseContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists();
            var blockBlob = _container.GetBlockBlobReference(BLOGMETAFILENAME);

            if (blockBlob.Exists())
            {
                var json = blockBlob.DownloadText();
                _blogs = JsonConvert.DeserializeObject<List<Blog>>(json);
            }
            else
            {
                _blogs = new List<Blog>();
            }
        }

        public void Save(Blog blogMeta)
        {
            _blogs.Add(blogMeta);
            SaveAll();
        }

        private void SaveAll()
        {
            var blockBlob = _container.GetBlockBlobReference(BLOGMETAFILENAME);
            blockBlob.UploadText(JsonConvert.SerializeObject(_blogs));
        }

        public Maybe<IList<Blog>> Get()
        {
            return _blogs.ToMaybe<IList<Blog>>();
        }

        public Maybe<Blog> Get(string blogUrl)
        {
            if (_blogs.Where(x => x.Url == blogUrl).Count() == 0)
            {
                return Maybe<Blog>.Nothing;
            }
            else
            {
                return _blogs.Where(x => x.Url == blogUrl).First().ToMaybe<Blog>();
            }
        }

        public void Delete(Blog blogMeta)
        {
            throw new NotImplementedException();
        }
    }
}

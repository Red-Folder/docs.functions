using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
    public class ToBeProcessedQueue : IToBeProcessed
    {
        private CloudBlobContainer _container;
        private CloudQueue _queue;

        public ToBeProcessedQueue(string connectionString, string containerName, string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists();

            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);
            _queue.CreateIfNotExists();
        }

        public void Add(string id, IList<Commit> commit)
        {
            var contents = JsonConvert.SerializeObject(commit);
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(id);
            cloudBlockBlob.UploadText(contents);

            CloudQueueMessage message = new CloudQueueMessage(id);
            _queue.AddMessage(message);
        }

        public IList<Commit> Get(string id)
        {
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(id);
            if (cloudBlockBlob.Exists())
            {
                var contents = cloudBlockBlob.DownloadText();
                return JsonConvert.DeserializeObject<IList<Commit>>(contents);
            }
            else
            {
                return null;
            }
        }

        public void MarkCompleted(string id)
        {
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(id);
            cloudBlockBlob.DeleteIfExists();
        }
    }
}

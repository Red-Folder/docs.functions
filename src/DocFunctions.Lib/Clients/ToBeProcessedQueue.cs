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
        private string _connectionString;
        private string _containerName;
        private string _queueName;

        public ToBeProcessedQueue(string connectionString, string containerName, string queueName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _queueName = queueName;
        }

        public void Add(string id, IList<Commit> commit)
        {
            var contents = JsonConvert.SerializeObject(commit);
            CloudBlockBlob cloudBlockBlob = GetContainer().GetBlockBlobReference(id);
            cloudBlockBlob.UploadText(contents);

            var queue = GetQueue();
            CloudQueueMessage message = new CloudQueueMessage(id);
            queue.AddMessage(message);
        }

        public IList<Commit> Get(string id)
        {
            CloudBlockBlob cloudBlockBlob = GetContainer().GetBlockBlobReference(id);
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
            CloudBlockBlob cloudBlockBlob = GetContainer().GetBlockBlobReference(id);
            cloudBlockBlob.DeleteIfExists();
        }

        private CloudBlobContainer GetContainer()
        {
            var container = BlobClient().GetContainerReference(_containerName);
            container.CreateIfNotExists();
            return container;
        }

        private CloudQueue GetQueue()
        {
            var queue = QueueClient().GetQueueReference(_queueName);
            queue.CreateIfNotExists();
            return queue;
        }

        private void DeleteQueue()
        {
            QueueClient().GetQueueReference(_queueName).DeleteIfExists();
        }

        private CloudStorageAccount StorageAccount()
        {
            return CloudStorageAccount.Parse(_connectionString);
        }

        private CloudBlobClient BlobClient()
        {
            return StorageAccount().CreateCloudBlobClient();
        }

        private CloudQueueClient QueueClient()
        {
            return StorageAccount().CreateCloudQueueClient();
        }

    }
}

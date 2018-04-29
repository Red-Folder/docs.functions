using DocFunctions.Lib.Clients;
using DocFunctions.Lib.Models.Github;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocFunctions.Lib.Integration.Clients
{
    public class ToBeProcessedQueueTests
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["ToBeProcessedStorage"].ToString();
        private string _containerName = ConfigurationManager.AppSettings["ToBeProcessedContainerName"] + Guid.NewGuid().ToString().ToLower();
        private string _queueName = ConfigurationManager.AppSettings["ToBeProcessedQueueName"] + Guid.NewGuid().ToString().ToLower();

        public ToBeProcessedQueueTests()
        {
            CreateContainer();
            CreateQueue();
        }

        ~ToBeProcessedQueueTests()
        {
            DeleteContainer();
            DeleteQueue();
        }

        [Fact]
        public void EndToEndTest()
        {
            var id = Guid.NewGuid().ToString();
            var commit = new Commit
            {
                Sha = Guid.NewGuid().ToString()
            };

            var sut = new ToBeProcessedQueue(_connectionString, _containerName, _queueName);

            // Add
            sut.Add(id, new List<Commit> { commit });

            // Get
            var result = sut.Get(id);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(commit.Sha, result[0].Sha);

            // Delete
            sut.MarkCompleted(id);

            // Get (to test its failed)
            result = sut.Get(id);
            Assert.Null(result);
        }

        private void CreateContainer()
        {
            BlobClient().GetContainerReference(_containerName).CreateIfNotExists();
        }

        private void DeleteContainer()
        {
            BlobClient().GetContainerReference(_containerName).DeleteIfExists();
        }

        private void CreateQueue()
        {
            QueueClient().GetQueueReference(_queueName).CreateIfNotExists();
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

using DocFunctions.Lib.Wappers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
    public class AzureBlobClient: IBlobClient
    {
        private CloudBlobContainer _container = null;

        public AzureBlobClient(string connectionString, string containerName)
        {
            InitialiseContainer(connectionString, containerName);
        }

        private void InitialiseContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            _container.CreateIfNotExists();
        }

        public void Upload(string filename, string contents)
        {
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(filename);
            cloudBlockBlob.UploadText(contents);
        }

        public void Upload(string filename, byte[] contents)
        {
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(filename);
            cloudBlockBlob.UploadFromByteArray(contents, 0, contents.Length);
        }

        public void Delete(string filename)
        {
            CloudBlockBlob cloudBlockBlob = _container.GetBlockBlobReference(filename);
            cloudBlockBlob.DeleteIfExists();
        }

        public void ClearAll()
        {
            var blobs = _container.ListBlobs();

            foreach(var blob in blobs)
            {
                ((CloudBlockBlob)blob).DeleteIfExists();
            }
        }
    }
}

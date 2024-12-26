using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace PersonalAccounting.Domain.Services
{
    public class BlobService
    {
        const string containerName = "receipts";
        private readonly ILogger<BlobService> logger;
        BlobServiceClient blobServiceClient;

        public BlobService(ILogger<BlobService> logger)
        {
            this.logger=logger;

            logger.LogDebug("Constructing blobService");
            // Retrieve the connection string for use with the application. 
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")!;

            blobServiceClient = new BlobServiceClient(connectionString);
            logger.LogDebug("blobServiceClient created . token ends with " + connectionString.Substring(connectionString.Length - 3));
        }

        public async Task<string> Upload(string fileName, byte[] file)
        {
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);


            fileName =$"{Path.GetFileNameWithoutExtension(fileName)}-{Guid.NewGuid()}{Path.GetExtension(fileName)}";

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file, overwrite the blob if it already exists
            await blobClient.UploadAsync(new BinaryData(file));
            return fileName;
        }

        public async Task<byte[]> Download(string fileName)
        {
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.DownloadContentAsync();
            return response.Value.Content.ToArray();
        }

        public async Task Delete(string fileName)
        {
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.DeleteAsync();
        }
    }
}

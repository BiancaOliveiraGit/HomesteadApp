using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HomesteadAzureFunctionApp
{
    public class BlobService
    {
        //TODO make IStorageService interface

        private IConfigurationRoot Configuration { get; set; }
        private string Connectionstring { get; set; }
        private CloudStorageAccount StorageAccount { get; set; }
        //private string PublicKey { get; set; }
        //private string PrivateKey { get; set; }
        //private string Email { get; set; }

        public BlobService(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        private void GetConnection()
        {
            //PublicKey = Configuration["Vapid:publicKey"];
            //PrivateKey = Configuration["Vapid:privateKey"];
            //Email = Configuration["Vapid:email"];
            Connectionstring = Configuration["AzureWebJobsStorage"];
            StorageAccount = CloudStorageAccount.Parse(Connectionstring);
        }

        public async Task<bool> PostBlobStorage(string data)
        {
            try
            {
                CloudBlobClient client;
                CloudBlobContainer container;
                CloudBlockBlob blob;

                GetConnection();

                client = StorageAccount.CreateCloudBlobClient();
                container = client.GetContainerReference("homestead-subscriptions");
                await container.CreateIfNotExistsAsync();

                blob = container.GetBlockBlobReference(data);
                blob.Properties.ContentType = "application/json";

                using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    await blob.UploadFromStreamAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
    }
}

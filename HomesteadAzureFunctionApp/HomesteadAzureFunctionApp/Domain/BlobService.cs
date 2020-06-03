
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomesteadAzureFunctionApp.Domain
{
    public class BlobService : IStorage
    {
        public IConfigurationRoot Configuration { get; set; }
        public string Connectionstring { get; set; }
        private CloudStorageAccount StorageAccount { get; set; }

        //private string PublicKey { get; set; }
        //private string PrivateKey { get; set; }
        //private string Email { get; set; }

        public BlobService(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public void GetConnection()
        {
            //PublicKey = Configuration["Vapid:publicKey"];
            //PrivateKey = Configuration["Vapid:privateKey"];
            //Email = Configuration["Vapid:email"];
            try
            {
                Connectionstring = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                //Connectionstring = Configuration["AzureWebJobsStorage"];
               // StorageAccount = CloudStorageAccount.Parse(Connectionstring);
                if(!CloudStorageAccount.TryParse(Connectionstring, out CloudStorageAccount _storageAccount))
                {
                    Console.WriteLine("Unable to parse connection string");
                    return;
                }
                StorageAccount = _storageAccount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PostBlobStorage(string data)
        {
            try
            {
                CloudBlobClient client;
                CloudBlobContainer container;
                CloudBlockBlob blob;
                string blobName = Guid.NewGuid() + "push-subcription";
                GetConnection();

                client = StorageAccount.CreateCloudBlobClient();
                container = client.GetContainerReference("homestead-subscriptions");
                await container.CreateIfNotExistsAsync();

                blob = container.GetBlockBlobReference(blobName);
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

        public async Task<List<string>> GetBlobStorage()
        {
            List<string> containerData = new List<string>();

            try
            {                
                CloudBlobClient client;
                CloudBlobContainer container;
                BlobContinuationToken continuationToken = null;
                BlobResultSegment resultSegment = null;
                GetConnection();

                client = StorageAccount.CreateCloudBlobClient();
                container = client.GetContainerReference("homestead-subscriptions");
              
                Console.WriteLine("Listing blobs...");
                do
                {
                    resultSegment = await container.ListBlobsSegmentedAsync(continuationToken);
                    // Do work here on resultSegment.Results
                    // Get block blobs because it has DownloadTextAsync method
                    containerData.AddRange(resultSegment.Results.OfType<CloudBlockBlob>().Select(b => b.DownloadTextAsync().Result).ToList());

                    continuationToken = resultSegment.ContinuationToken;
                } while (continuationToken != null);
            }
            catch (Exception ex)
            {
                throw;
            }

            return containerData;
        }

    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomesteadAzureFunctionApp.Commands
{
    public class BlobStorage
    {
        public string ConnectionString { get; set; }
    
        public BlobStorage(IConfigurationRoot config)
        {
            ConnectionString = config.GetWebJobsConnectionString("AzureWebJobsStorage");
        }

        public async Task<bool> BlobStoragePost(string data)
        {
            bool isSucceed = true;
            try
            {

            }
            catch (Exception)
            {
                return false;
            }
            return isSucceed;
        }
    }
}

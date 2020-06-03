using Microsoft.Extensions.Configuration;

namespace HomesteadAzureFunctionApp.Domain
{
    interface IStorage
    {
        IConfigurationRoot Configuration { get; set; }
        string Connectionstring { get; set; }

        void GetConnection();

    }
}

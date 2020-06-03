using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace HomesteadAzureFunctionApp.API
{
    public static class StartupConfiguration
    {
        public static IConfigurationRoot GetConfiguration(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
        }
    }
}

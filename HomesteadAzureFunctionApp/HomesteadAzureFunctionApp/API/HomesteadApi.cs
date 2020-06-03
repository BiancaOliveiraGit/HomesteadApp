using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using HomesteadAzureFunctionApp.API;
using HomesteadAzureFunctionApp.Domain;
using System.Linq;

namespace HomesteadAzureFunctionApp
{
    public static class HomesteadApi
    {
        [FunctionName("GetSeasonData")]
        public static async Task<IActionResult> RunGetAllSeasons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/seasons")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("HomesteadApi/GetSeasonData HTTP trigger function processed a request.");

            // Using this setup allows Environment Variables to be set via local settings in development, and App Settings in Azure
            var config = StartupConfiguration.GetConfiguration(context);
            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            List<SeasonData> seasonDatas = MockDB.GetSeasonData();

            return seasonDatas != null
                ? (ActionResult)new OkObjectResult(seasonDatas)
                : new BadRequestObjectResult("Error. No Season Data Found.");
        }

        [FunctionName("SaveSubscription")]
        public static async Task<IActionResult> RunSaveSubscription(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/subscriptions")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("HomesteadApi/SaveSubscription HTTP trigger function processed a request.");

            // Using this setup allows Environment Variables to be set via local settings in development, and App Settings in Azure
            var config = StartupConfiguration.GetConfiguration(context);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            var blobStorage = new BlobService(config);
            var isSaved = await blobStorage.PostBlobStorage(requestBody);

            return isSaved 
                ? (ActionResult)new OkObjectResult(isSaved)
                : new BadRequestObjectResult("Error. Error on posting subscription to blob storage");
        }

        [FunctionName("GetSubscription")]
        public static async Task<IActionResult> RunGetSubscription(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/subscriptions")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("HomesteadApi/getSubscription HTTP trigger function processed a request.");

            // Using this setup allows Environment Variables to be set via local settings in development, and App Settings in Azure
            var config = StartupConfiguration.GetConfiguration(context);

            var blobStorage = new BlobService(config);
            var subscriptions = await blobStorage.GetBlobStorage();
            //TODO serialize subscriptions to PushSubscriptions

            return subscriptions.Any()
                ? (ActionResult)new OkObjectResult(subscriptions)
                : new BadRequestObjectResult("Error. Error on get subscriptions from blob storage");
        }

        [FunctionName("TemplateFunction")]
        public static async Task<IActionResult> RunTemplate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using HomesteadAzureFunctionApp.Dto;
using WebPush;

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
            var config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            List<SeasonData> seasonDatas = MockSeasonDB.GetSeasonData();

            return seasonDatas != null
                ? (ActionResult)new OkObjectResult(seasonDatas)
                : new BadRequestObjectResult("Error. No Season Data Found.");
        }

        [FunctionName("PostSubscription")]
        public static async Task<IActionResult> RunPostSubscription(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/subscription")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("HomesteadApi/PostSubscription HTTP trigger function processed a post request.");

            // Using this setup allows Environment Variables to be set via local settings in development, and App Settings in Azure
            var config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();


            //string name = req.Query["name"];
           
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            List<SeasonData> seasonDatas = MockSeasonDB.GetSeasonData();

            return seasonDatas != null
                ? (ActionResult)new OkObjectResult(seasonDatas)
                : new BadRequestObjectResult("Error. No Season Data Found.");
        }

        [FunctionName("PushNotificationHomestead")]
        public static async Task<IActionResult> RunPushNotificationHomestead(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/push-notification")] HttpRequest req, ILogger log, ExecutionContext context)
        {
            log.LogInformation("HomesteadApi/RunPushNotificationHomestead HTTP trigger function processed a request.");

            // Using this setup allows Environment Variables to be set via local settings in development, and App Settings in Azure
            var config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            //string name = req.Query["name"];
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PushPayloadDto data = JsonConvert.DeserializeObject<PushPayloadDto>(requestBody);

            //setup vapid details from localSettings - TODO Get from secret keys on Azure
            var vapidDetails = new VapidDetails(
                config["Vapid:Email"],
                config["Vapid:PublicKey"],
                config["Vapid:PrivateKey"]
                );
            //PushSubscription
            //return seasonDatas != null
            //    ? (ActionResult)new OkObjectResult(seasonDatas)
            //    : new BadRequestObjectResult("Error. No Season Data Found.");
        }

        [FunctionName("TemplateFunction")]
        public static async Task<IActionResult> RunTemplate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext context)
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

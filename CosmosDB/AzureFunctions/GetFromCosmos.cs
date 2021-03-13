using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class GetFromCosmos
    {
        [FunctionName("GetFromCosmos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
            databaseName: "IOT20", 
            collectionName: "Measurements", 
            CreateIfNotExists = true, 
            ConnectionStringSetting = "CosmosDB",
            SqlQuery = "SELECT * FROM c ORDER BY c.epochtime DESC OFFSET 0 LIMIT 10 ")] IEnumerable<DhtMeasurement>  cosmos,
            ILogger log)
        {
            return new OkObjectResult(cosmos);
        }
    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionsTable
{
    public static class GetDataFromTable
    {
        [FunctionName("GetDataFromTable")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("Measurements")] CloudTable cloudtable,
            ILogger log)
        {
            string limit = req.Query["limit"];
            string orderby = req.Query["orderby"];

            IEnumerable<FakeData> results = 
                await cloudtable.ExecuteQuerySegmentedAsync(new TableQuery<FakeData>(), null);

            if (orderby == "desc")
                results = results.OrderByDescending(ts => ts.Fibonacci);

            if (limit != null)
                results = results.Take(Int32.Parse(limit));

            return results != null
                ? (ActionResult)new OkObjectResult(results)
                : new BadRequestObjectResult("[]");

        }
    }
}


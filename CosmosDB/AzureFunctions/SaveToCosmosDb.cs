using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class SaveToCosmosDb
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToCosmosDb")]
        public static void Run(
            [IoTHubTrigger("messages/events", Connection = "IotHub", ConsumerGroup = "cosmos")] EventData message,
            [CosmosDB(databaseName: "IOT20", collectionName: "Measurements", CreateIfNotExists = true, ConnectionStringSetting = "CosmosDB")] out dynamic cosmos,
            ILogger log)
        {
            var msg = JsonConvert.DeserializeObject<DhtMeasurement>(cosmos = Encoding.UTF8.GetString(message.Body.Array));
            msg.DeviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
            msg.name = message.Properties["name"].ToString();
            msg.school = message.Properties["school"].ToString();

            var json = JsonConvert.SerializeObject(msg);
            cosmos = json;

        }
    }
}
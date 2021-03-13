using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace AzureFunctionsTable
{
    public static class SaveToTable
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToTable")]
        [return: Table("Measurements")]
        public static FakeData Run([IoTHubTrigger("messages/events", Connection = "IotHub", ConsumerGroup = "table")] EventData message, ILogger log)
        {
            var msg = JsonConvert.DeserializeObject<FakeData>(Encoding.UTF8.GetString(message.Body.Array));
            msg.DeviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
            msg.name = message.Properties["name"].ToString();
            msg.school = message.Properties["school"].ToString();
            msg.PartitionKey = "IOT";
            msg.RowKey = Guid.NewGuid().ToString();

            return msg;
        }
    }
}
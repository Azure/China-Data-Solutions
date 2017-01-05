using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataCollectingRole
{
    public class DataSimulator
    {
        public async void Run()
        {
            var connStr = "Endpoint=sb://mediaanalysishub.servicebus.chinacloudapi.cn/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SMvFAK3qek7fg5LzTzn2zSnfUKkiT1LfKEi84uYr19Q=";
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connStr);
            var ed = namespaceManager.CreateEventHubIfNotExists("newshub");
            var client = EventHubClient.CreateFromConnectionString(connStr, "newshub");

            var text = File.ReadAllText(@"data\samplenews.json");
            var objects = JArray.Parse(text);

            foreach (var item in objects)
            {
                var bytes = Encoding.UTF8.GetBytes(item.ToString());
                dynamic obj = JObject.Parse(item.ToString());
                EventData data = new EventData(bytes)
                {
                    PartitionKey = obj.Id
                };
                try
                {
                    await client.SendAsync(data);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}

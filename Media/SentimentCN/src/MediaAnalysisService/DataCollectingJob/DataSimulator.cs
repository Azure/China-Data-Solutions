using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using MediaAnalysis;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;

namespace DataCollectingJob
{
    public class DataSimulator
    {
        public async void Run()
        {
            var connStr = ConfigurationManager.AppSettings["EventHubConnection"];
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Wrong Eventhub Connection Configuration");

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connStr);
            var ed = namespaceManager.CreateEventHubIfNotExists("newshub");
            var client = EventHubClient.CreateFromConnectionString(connStr, "newshub");


            var text = File.ReadAllText(@"data\samplenews.json");
            var objects = JArray.Parse(text);

            int index = 1;
            foreach (var item in objects)
            {
                var bytes = Encoding.UTF8.GetBytes(item.ToString());
                dynamic obj = JObject.Parse(item.ToString());
                var data = new EventData(bytes)
                {
                    PartitionKey = obj.Id
                };
                try
                {
                    await client.SendAsync(data);
                    if (index%100 == 0)
                    {
                        Logger.Log($"{index} items sent.");
                        index++;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            //// Block this thread will not run again
            while (true)
            {
                System.Threading.Thread.Sleep(100000);
            }
        }
    }
}
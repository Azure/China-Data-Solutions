using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Drawing;
using Microsoft.ServiceBus.Messaging;
using DataAccessLayer.Data;

namespace MessageHandler
{
    public class EventHubMessageProcessor
    {
        public void StartMessageProcess()
        {
            string eventHubName = ConfigurationManager.AppSettings["EventHubName"];
            string eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnection"];
            string consumerGroupSetting = ConfigurationManager.AppSettings["EventHubConsumerGroup"];
            int eventHubPartitions = Int32.Parse(ConfigurationManager.AppSettings["EventHubPartitions"]);

            Console.WriteLine("Message Processing Started");

            try
            {
                var eventhubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
                var ehConsumerGroup = eventhubClient.GetConsumerGroup(consumerGroupSetting);
                var cts = new CancellationTokenSource();

                // start listening all event hub partitions
                for (int i = 0; i < eventHubPartitions; i++)
                {
                    Task.Factory.StartNew((state) =>
                    {
                        Debug.WriteLine("Starting worker to process partition: {0}", state);
                        var receiver = ehConsumerGroup.CreateReceiver(state.ToString(), DateTime.UtcNow.AddDays(-10));
                        while (true)
                        {
                            try
                            {
                                var eventData = receiver.Receive();
                                if (eventData == null)
                                {
                                    System.Threading.Thread.Sleep(300);
                                    continue;
                                }
                                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                                JObject obj = JObject.Parse(data);
                                var deviceId = (string)obj["moduleId"];
                                if (!string.IsNullOrWhiteSpace(deviceId))
                                {
                                    var type = "Alert";
                                    var name = "UnSafe Action Detected";
                                    using (VideoAnalyticContext db = new VideoAnalyticContext())
                                    {
                                        Event e = new Event
                                        {
                                            Body = data,
                                            Source = deviceId,
                                            Type = type,
                                            Name = name,
                                            Time = DateTime.UtcNow
                                        };
                                        db.Events.Add(e);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Exception in EventHubReader! Message = " + ex.Message);
                            }
                            if (cts.IsCancellationRequested)
                            {
                                Debug.WriteLine("Stopping: {0}", state);
                                receiver.Close();
                            }
                        }
                    }, i);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
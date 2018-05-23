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

namespace Iotc.Web.Backend.Models
{
    public class EHReader
    {
        string eventHubName = ConfigurationManager.AppSettings["EventHubName"];
        string eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnection"];
        string consumerGroupSetting = ConfigurationManager.AppSettings["EventHubConsumerGroup"];
        int eventHubPartitions = Int32.Parse(ConfigurationManager.AppSettings["EventHubPartitions"]);
        private CancellationTokenSource cts;
        private static string _imageData;

        private int _readFlag;
        public CacheData cache;
        private string _imagePath = null;
        private string _trigger = null;
        private string _currentTime = null;

        public EHReader(string imagePath)
        {
            try
            {
                _imagePath = imagePath;
                var eventhubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
                var ehConsumerGroup = eventhubClient.GetConsumerGroup(consumerGroupSetting);
                cts = new CancellationTokenSource();
                cache = new CacheData();

                _readFlag = 0;

                // start listening all event hub partitions
                for (int i = 0; i < eventHubPartitions; i++)
                {
                    Task.Factory.StartNew((state) =>
                        {
                            Debug.WriteLine("Starting worker to process partition: {0}", state);
                            var receiver = ehConsumerGroup.CreateReceiver(state.ToString(), DateTime.UtcNow);
                            while (true)
                            {
                                try
                                {
                                    var eventData = receiver.Receive();
                                    if (eventData == null)
                                        continue;
                                    string data = Encoding.UTF8.GetString(eventData.GetBytes());
                                    JObject obj = JObject.Parse(data);
                                    var deviceId = (string)obj["deviceId"];
                                    if (string.IsNullOrEmpty(deviceId))
                                    {
                                        deviceId = (string)obj["moduleId"];
                                    }
                                    var text = $"Device Id :{deviceId} ; Message Id: {obj["messageId"]}; ";
                                    var picture = (string)obj["snapshot"];
                                    var linkText = (string)obj["video"];

                                    var image = getImageFromUrl(picture);
                                    _imageData = image;

                                    //_readFlag = 1;
                                    //int r = ComparePicture();

                                    TrigerAlarm(deviceId,text, image, linkText);
                                    _readFlag = 0;

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

        public string getImageFromUrl(string url)
        {
            try
            {
                var localPath = $"{_imagePath}/query/q.png";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), localPath);

                }

                using (Image image = Image.FromFile(localPath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in getImageFromUrl = " + ex.Message);
                return null;
            }

        }

        public string getData(string time)
        {
            if (time == _currentTime)
                return null;

            return _imageData;

        }

        public void clearData()
        {
            _readFlag = 0;
            cache.ClearData();
            _currentTime = null;
            _imageData = null;
        }


        public int getStatus()
        {
            return _readFlag;
        }

        public string GetTrigger()
        {
            return _trigger;
        }

        public void TrigerAlarm(string deviceId,string text, string image, string linkText)
        {
            _trigger = "ON";
            cache.AddData(deviceId, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), image, text, linkText);
        }

    }
}
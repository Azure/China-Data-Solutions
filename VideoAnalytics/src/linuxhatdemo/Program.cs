namespace linuxhatdemo
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using Microsoft.Azure.Devices.Shared;
    using Newtonsoft.Json;
    using OpenCvSharp;
    using System.Diagnostics;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System.IO;

    class Program
    {
        private static VideoCapture _cameraCapture;
        private static HOGDescriptor _hog;

        private static double _fps = 30.0;
        private const int CAPTUREWIDTH = 640;
        private const int CAPTUREHEIGHT = 320;
        private static ModuleClient _moduleClient;
        private static Random _random = new Random();

        private static Mat latestFrame = new Mat();
        private static List<Mat> matBuffer = new List<Mat>();
        private const int bufferSize = 300;

        private static String moduleId = "default";
        private static String RTSP = "";
        private static int counter = 0;

        private static int failure = 0;
        private const int MAXFAILTIMES = 40;
        private static Object frameLoker = new Object();
        private static Object captureLocker = new Object();

        private static String MOUNTVOLUME = "./{0}";
        private static String STORAGEURI = "";
        private static String STORAGECONNECTSTRING = "";

        private static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            try
            {
                LogUtil.Log("start", LogLevel.Info);

                Init().Wait();
                
                LogUtil.Log("Init finished!", LogLevel.Info);

                // --for debug only--
                // RTSP = "rtsp://admin:Passw0rd!@10.172.94.234:554/h264/ch1/main/av_stream";
                // RTSP = "rtsp://23.102.236.116:5554/test2.mpg";

                _cameraCapture = new VideoCapture(RTSP);

                _hog = new HOGDescriptor();
                _hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());


                Task.Run(async () =>
                {
                    while (true)
                    {
                        var tRtsp = "";
                        lock (captureLocker)
                        {
                            tRtsp = RTSP;
                        }

                        if (!String.IsNullOrEmpty(tRtsp) && _cameraCapture != null)
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(2000);
                            LogUtil.Log("Check RTSP", LogLevel.Warning);
                        }

                    }

                    while (true)
                    {
                        await GetFrame();
                    }
                });

                Task.Run(async () =>
                {
                    while (true)
                    {
                        await ProcessFrame();
                        await Task.Delay(1000);
                    }
                }).Wait();

                LogUtil.Log("Main Finshed!");

            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }
        }


        private static async Task ProcessFrame()
        {
            Mat mat = null;

            try
            {
                lock (frameLoker)
                {
                    mat = latestFrame.Clone();
                }

                if (mat.Empty())
                {
                    LogUtil.Log("Empty Mat !!!", LogLevel.Warning);
                    mat.Dispose();
                    return;
                }

                var filename = String.Format("{1}-orignal-{0}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"), moduleId);

                LogUtil.Log(filename, LogLevel.Info);


                var result = _hog.Detect(mat);
                if (result.Length > 0)
                {
                    LogUtil.Log("Persons Detected", LogLevel.Info);

                    //TODO for debug only
                    // mat.SaveImage(filename);

                    var hatRresult = await FaceUtil.DetectHat(mat.ToBytes());

                    LogUtil.Log("hat Result: " + hatRresult, LogLevel.Info);

                    if (!hatRresult)
                    {
                        var imageName = String.Format("{1}-alert-{0}.png", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"), moduleId);
                        var imageNamelocal = BuildLocalFilePath(imageName);
                        mat.SaveImage(imageNamelocal);

                        var videoname = String.Format("{1}-record-{0}.avi", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"), moduleId);
                        var videonamelocal = BuildLocalFilePath(videoname);


                        await UploadFile(imageName, imageNamelocal);

                        await SendEvent(String.Format(STORAGEURI, imageName), String.Format(STORAGEURI, videoname));


                        await RecordVideo(videonamelocal, 2);

                        await UploadFile(videoname, videonamelocal);

                        if (File.Exists(imageNamelocal))
                        {
                            File.Delete(imageNamelocal);
                        }
                        if (File.Exists(videonamelocal))
                        {
                            File.Delete(videonamelocal);
                        }

                    }

                }

            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }
            finally
            {
                if (mat != null) mat.Dispose();
            }
        }

        private static string BuildLocalFilePath(string filename)
        {
            return String.Format(MOUNTVOLUME, filename);

        }

        private static async Task SendEvent(string snapshot, string video)
        {
            try
            {
                LogUtil.Log("start SendEvent", LogLevel.Info);

                int counterValue = Interlocked.Increment(ref counter);

                string dataBuffer;

                LogUtil.Log("Device sending messages to IoTHub...", LogLevel.Info);


                dataBuffer = string.Format("{{\"moduleId\":\"{0}\",\"messageId\":{1},\"snapshot\":\"{2}\",\"video\":\"{3}\"}}", moduleId, counterValue, snapshot, video);
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                //eventMessage.Properties.Add("Snapshot", snapshot);

                await _moduleClient.SendEventAsync("output1", eventMessage);

                LogUtil.Log("sending messages success", LogLevel.Info);
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }

        }


        private static async Task UploadFile(string filename, string filepath)
        {
            try
            {
                LogUtil.Log($"start upload: {filename}", LogLevel.Info);

                string storageConnectionString = STORAGECONNECTSTRING;
                CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
                CloudBlobClient serviceClient = account.CreateCloudBlobClient();

                // Create container. Name must be lower case.
                var container = serviceClient.GetContainerReference("upload");
                await container.CreateIfNotExistsAsync();

                // write a blob to the container
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);
                await blob.UploadFromFileAsync(filepath);

                LogUtil.Log($"uploadted", LogLevel.Info);
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }

        }

        static Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                LogUtil.Log("Desired property change:", LogLevel.Info);
                LogUtil.Log(JsonConvert.SerializeObject(desiredProperties), LogLevel.Info);

                if (desiredProperties["RTSP"] != null)
                {
                    string tRTSP = desiredProperties["RTSP"];
                    LogUtil.Log("RTSP:" + tRTSP, LogLevel.Info);

                    lock (captureLocker)
                    {
                        if (_cameraCapture != null)
                        {
                            _cameraCapture.Dispose();
                        }
                        _cameraCapture = new VideoCapture(tRTSP);

                        RTSP = tRTSP;
                    }
                    LogUtil.Log("RTSP changed", LogLevel.Info);
                }
                if (desiredProperties["StorageConnectString"] != null)
                {

                    STORAGECONNECTSTRING = desiredProperties["StorageConnectString"];
                    LogUtil.Log($"StorageConnectString: {STORAGECONNECTSTRING}", LogLevel.Info);
                }
                if (desiredProperties["StorageURI"] != null)
                {

                    STORAGEURI = desiredProperties["StorageURI"];
                    LogUtil.Log($"StorageURI: {STORAGEURI}", LogLevel.Info);
                }
                if (desiredProperties["ModuleId"] != null)
                {

                    moduleId = desiredProperties["ModuleId"];
                    LogUtil.Log($"ModuleId: {moduleId}", LogLevel.Info);
                }


            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    LogUtil.LogException(exception);
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
            return Task.CompletedTask;
        }


        /// <summary>
        /// Initializes the DeviceClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static async Task Init()
        {
            try
            {

                AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
                ITransportSettings[] settings = { amqpSetting };

                // Open a connection to the Edge runtime
                _moduleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
                await _moduleClient.OpenAsync();
                LogUtil.Log("IoT Hub module client initialized.",LogLevel.Info);

                var moduleTwin = await _moduleClient.GetTwinAsync();

                moduleId = moduleTwin.ModuleId; // not work yet.
                LogUtil.Log($"moduleTwin.moduleId: {moduleId}");

                var moduleTwinCollection = moduleTwin.Properties.Desired;
                if (moduleTwinCollection["ModuleId"] != null)
                {

                    moduleId = moduleTwinCollection["ModuleId"];
                    LogUtil.Log($"ModuleId: {moduleId}", LogLevel.Info);
                }

                if (moduleTwinCollection["RTSP"] != null)
                {

                    string tRTSP = moduleTwinCollection["RTSP"];
                    LogUtil.Log($"RTSP: {tRTSP}", LogLevel.Info);

                    lock (captureLocker)
                    {
                        if (_cameraCapture != null)
                        {
                            _cameraCapture.Dispose();
                        }
                        _cameraCapture = new VideoCapture(tRTSP);

                        RTSP = tRTSP;
                    }
                    LogUtil.Log("RTSP Set", LogLevel.Info);

                }
                if (moduleTwinCollection["StorageConnectString"] != null)
                {

                    STORAGECONNECTSTRING = moduleTwinCollection["StorageConnectString"];
                    LogUtil.Log($"StorageConnectString: {STORAGECONNECTSTRING}", LogLevel.Info);
                }
                if (moduleTwinCollection["StorageURI"] != null)
                {

                    STORAGEURI = moduleTwinCollection["StorageURI"];
                    LogUtil.Log($"StorageURI: {STORAGEURI}", LogLevel.Info);
                }

                await _moduleClient.OpenAsync();

                LogUtil.Log("IoT Hub module client initialized.", LogLevel.Info);

                // Register callback to be called when a message is received by the module
                await _moduleClient.SetInputMessageHandlerAsync("input1", PipeMessage, _moduleClient);

                // Attach callback for Twin desired properties updates
                await _moduleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);


            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }
        }

        private static async Task GetFrame()
        {
            Mat frame = null;
            Mat newFrame = null;
            try
            {
                frame = _cameraCapture.RetrieveMat();

                if (frame != null && (!frame.Empty()))
                {
                    newFrame = frame.Resize(new Size(CAPTUREWIDTH, CAPTUREHEIGHT));

                    lock (frameLoker)
                    {
                        if (latestFrame == newFrame)
                        {
                            LogUtil.Log("latest frame the same !!!!!!");
                        }
                        if (latestFrame != null) latestFrame.Dispose();
                        latestFrame = newFrame.Clone();

                        matBuffer.Add(newFrame.Clone());

                        if (matBuffer.Count >= bufferSize)
                        {
                            var firstmat = matBuffer[0];
                            matBuffer.RemoveAt(0);
                            firstmat.Dispose();
                        }
                    }
                }
                else
                {
                    if (frame != null) frame.Dispose();
                    failure++;
                    LogUtil.Log($"failed {failure} times.", LogLevel.Warning);
                }

                if (failure >= MAXFAILTIMES)
                {
                    LogUtil.Log($"Begin to Reset VideoCapture after failed {MAXFAILTIMES} times.", LogLevel.Warning);
                    await Task.Delay(5000);
                    lock (captureLocker)
                    {
                        if (_cameraCapture != null)
                        {
                            _cameraCapture.Dispose();
                        }
                        _cameraCapture = new VideoCapture(RTSP);
                    }
                    failure = 0;
                    LogUtil.Log($"Reset VideoCapture after failed {MAXFAILTIMES} times.", LogLevel.Warning);
                }
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }
            finally
            {
                if (frame != null) frame.Dispose();
                if (newFrame != null) newFrame.Dispose();
            }
        }


        static async Task<MessageResponse> PipeMessage(Message message, object userContext)
        {
            try
            {
                LogUtil.Log("PipeMessage Received message", LogLevel.Info);

                int counterValue = Interlocked.Increment(ref counter);

                var moduleClient = userContext as ModuleClient;
                if (moduleClient == null)
                {
                    throw new InvalidOperationException("UserContext doesn't contain " + "expected values");
                }

                byte[] messageBytes = message.GetBytes();
                string messageString = Encoding.UTF8.GetString(messageBytes);

                LogUtil.Log($"Received message: {counterValue}, Body: [{messageString}]", LogLevel.Info);

                if (!string.IsNullOrEmpty(messageString))
                {
                    var pipeMessage = new Message(messageBytes);
                    foreach (var prop in message.Properties)
                    {
                        pipeMessage.Properties.Add(prop.Key, prop.Value);
                    }

                    await moduleClient.SendEventAsync("output1", pipeMessage);

                    LogUtil.Log("Received message sent to output1", LogLevel.Info);

                }
                return MessageResponse.Completed;
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
                return MessageResponse.None;
            }
        }


        private static async Task RecordVideo(string videoname, int seconds)
        {
            VideoWriter writer = null;
            try
            {
                await Task.Delay(seconds * 1000);

                writer = new VideoWriter(videoname, VideoWriter.FourCC('M', 'J', 'P', 'G'), _fps, new Size(CAPTUREWIDTH, CAPTUREHEIGHT), true);

                lock (frameLoker)
                {

                    LogUtil.Log($"start write buffer", LogLevel.Info);

                    foreach (var buffer in matBuffer)
                    {
                        writer.Write(buffer);
                    }
                }

                LogUtil.Log($"{videoname} Record {seconds} seconds finished", LogLevel.Info);
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
            }

            finally
            {
                if (writer != null) writer.Dispose();
            }

        }

    }
}

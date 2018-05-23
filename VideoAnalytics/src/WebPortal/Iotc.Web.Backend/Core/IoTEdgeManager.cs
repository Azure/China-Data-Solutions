using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Iotc.Web.Backend.Models
{
    public class IoTEdgeManager
    {
        private static readonly string iotHubConnectionString = ConfigurationManager.AppSettings["IoTHubConnectionString"];
        private static readonly string iotHubHostName = ConfigurationManager.AppSettings["IoTHubHostName"];
        private static readonly string storageURI = ConfigurationManager.AppSettings["StorageURI"];
        private static readonly string storageConnectString = ConfigurationManager.AppSettings["StorageConnectString"];
        private readonly static string moduleImageUri = ConfigurationManager.AppSettings["ModuleImageUri"];


        public static async Task<string> AddDeviceAsync(string deviceName)
        {
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            var device = await manager.AddDeviceAsync(new Device(deviceName) { Capabilities = new DeviceCapabilities() { IotEdge = true } });

            var key = device.Authentication.SymmetricKey.PrimaryKey;

            var connectStr = $"HostName={iotHubHostName};DeviceId={deviceName};SharedAccessKey={key}";

            LogUtil.Log("Device Added");
            LogUtil.Log(connectStr);
            return connectStr;
        }

        public static async Task DeleteDeviceAsync(string deviceName)
        {
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            await manager.RemoveDeviceAsync(deviceName);
            LogUtil.Log("Device Removed");
        }


        public static async Task CheckModuleStatesOnDeviceAsync(string devicenName)
        {
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);

            var modules = await manager.GetModulesOnDeviceAsync(devicenName);
            foreach (var module in modules)
            {
                LogUtil.Log($"{module.Id} : {module.ConnectionState}");

            }

        }

        public static async Task<bool> CheckDeviceStatesAsync(string deviceName)
        {
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            var device = await manager.GetDeviceAsync(deviceName);
            var modulesOnDevice = await manager.GetModulesOnDeviceAsync(deviceName);
            int connectedModules = 0;
            foreach (var module in modulesOnDevice)
            {
                if (module.ConnectionState == DeviceConnectionState.Connected)
                {
                    connectedModules++;
                }
            }

            if (device.ConnectionState == DeviceConnectionState.Connected || connectedModules > 0)
            {
                return true;
            }

            return false;
        }

        public static async Task<string> AddModuleOnDeviceAsync(string moduleName, string deviceName, string pipelineName, JObject properties, JObject moduleContent)
        {
            pipelineName = pipelineName.ToLower().Replace(" ", string.Empty);
            var contentStr = string.Empty;
            switch (pipelineName)
            {
                case "workplacesafety":
                    contentStr = BuildContentForAddWorkplaceSafety(moduleName, properties, moduleContent, moduleImageUri);
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(contentStr))
            {
                return null;
            }

            var content = JsonConvert.DeserializeObject<ConfigurationContent>(contentStr);
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            await manager.ApplyConfigurationContentOnDeviceAsync(deviceName, content);

            return contentStr;

        }

        private static string BuildContentForAddWorkplaceSafety(string moduleName, JObject properties, JObject moduleContent, string imageUri)
        {
            var rtsp = properties?.GetValue("RTSP")?.Value<string>();
            if (string.IsNullOrEmpty(rtsp) || string.IsNullOrEmpty(storageURI) || string.IsNullOrEmpty(storageConnectString))
            {
                LogUtil.Log("BuildContentForAddWorkplaceSafety with not valiad properties");
                return string.Empty;
            }

            var moduleClass = new WorkplacesafetyModule(moduleName, rtsp, storageURI, storageConnectString, imageUri);

            var modulePart = new JObject {
                {moduleName, moduleClass.GetModules()}
            };

            var otherPart = new JObject {
                {moduleName,moduleClass.GetProperties()}
            };

            JObject modules = moduleContent["moduleContent"]["$edgeAgent"]["properties.desired"]["modules"] as JObject;
            modules.Add(moduleName, moduleClass.GetModules());
            JObject others = moduleContent["moduleContent"] as JObject;
            others.Add(moduleName, moduleClass.GetProperties());

            LogUtil.Log($"BuildContentForAddWorkplaceSafety: {moduleContent.ToString()}");
            return moduleContent.ToString();
        }


        public static async Task<string> DeleteModuleOnDeviceAsync(string moduleName, string deviceName, string pipelineName, JObject moduleContent)
        {
            pipelineName = pipelineName.ToLower().Replace(" ", string.Empty);

            JObject modules = moduleContent["moduleContent"]["$edgeAgent"]["properties.desired"]["modules"] as JObject;
            modules.Property(moduleName).Remove();
            JObject others = moduleContent["moduleContent"] as JObject;
            others.Property(moduleName).Remove();

            var contentStr = moduleContent.ToString();
            var content = JsonConvert.DeserializeObject<ConfigurationContent>(contentStr);
            RegistryManager manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            await manager.ApplyConfigurationContentOnDeviceAsync(deviceName, content);

            return contentStr;
        }
    }
}
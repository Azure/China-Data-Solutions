using Newtonsoft.Json.Linq;


namespace Iotc.Web.Backend.Models
{
    public class WorkplacesafetyModule : BaseModule
    {
        public WorkplacesafetyModule(string moduleName,string rtsp, string storageUri, string storageConnectString, string imageUri):base()
        {
            Settings.Image = imageUri;
            var properties = new JObject {
                {"RTSP", rtsp},
                {"ModuleId",moduleName},
                {"StorageURI",storageUri},
                {"StorageConnectString",storageConnectString}
            };

            Properties = properties;
        }

    }
}
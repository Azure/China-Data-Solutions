using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Iotc.Web.Backend.Models
{
    public class BaseModule
    {
        public string Version { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string RestartPolicy { get; set; }
        public ModuleSettings Settings { get; set; }

        public JObject Properties { get; set; }

        public BaseModule()
        {
            Version = "1.0";
            Type = "docker";
            Status = "running";
            RestartPolicy = "always";
            Settings = new ModuleSettings()
            {
                CreateOptions = "{}"
            };
            Properties = new JObject();
        }

        public JObject GetModules()
        {
            JObject result = new JObject
            {
                {"version",Version},
                { "type",Type},
                {"status",Status},
                { "restartPolicy",RestartPolicy},
                { "settings",new JObject{
                    { "image",Settings.Image },
                    { "createOptions",Settings.CreateOptions}
               } }
            };
            return result;
        }
        public JObject GetProperties()
        {
            JObject result = new JObject
            {
                { "properties.desired", Properties }
            };

            return result; ;
        }
    }

    public class ModuleSettings
    {
        public string Image { get; set; }
        public string CreateOptions { get; set; }
    }

}

namespace linuxhatdemo
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class FaceUtil
    {
        private const string subscriptionKey = "";
        private const string uriBase = "http://vamvpcls.eastus.cloudapp.azure.com/api/detect";

        public static async Task<bool>  DetectHat(Byte[] imageData)
        {
            try
            {
                // Execute the REST API call.
                // if error , not alert
                var responseObjects = await MakeAnalysisRequest(imageData);

                bool result = true;

                foreach (var jResponseOject in responseObjects)
                {
                    var hair = jResponseOject?["hasHat"]?.Value<bool>() ?? true; 
                    if (hair == false)
                    {
                        result = false;
                    }

                }
                return result;
            }
            catch(Exception e)
            {
                LogUtil.LogException(e);
                return true;
            }
        }

        private static async Task<JArray> MakeAnalysisRequest(Byte[] imageData)
        {
            try
            {
                HttpClient client = new HttpClient(){
                    Timeout = TimeSpan.FromSeconds(10)
                };

                string uri = uriBase;

                HttpResponseMessage response;

                // Request body.
                using (ByteArrayContent content = new ByteArrayContent(imageData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);

                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();
                    var persons = JObject.Parse(contentString);

                    return persons["persons"]?.Value<JArray>() ?? null;
                }
            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
                return null;
            }
        }

    }
}

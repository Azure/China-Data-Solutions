using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NLPLib.Entitylinking
{
    public class EntityLinking
    {
        public async Task<EntityLinkingResponse> Extract(string text)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://mcdisf.chinanorth.cloudapp.chinacloudapi.cn");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", text)
            });
            HttpResponseMessage response = await client.PostAsync($"IntelligentService/SentimentService/api/entitylinking/extract", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<EntityLinkingResponse>();
                return result;
            }
            else
            {
                return null;

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;
using EntityFramework.BulkInsert.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataLibrary.Pipeline.WeiboAnalysis
{
    public class KOLAnalysisActivity : IPipelineActivity
    {
        public string Name { get; set; } = "KOLAnalysis";

        public async Task<ActivityResult> Run(PipelineContext context)
        {
            Logger.Log("Start KOL Analysis");
            var result = new ActivityResult();
            var ip = ConfigurationManager.AppSettings["IPAddress"];
            var user = ConfigurationManager.AppSettings["RServerUserName"];
            var password = ConfigurationManager.AppSettings["RServerPassword"];

            var loginContet = $@"{{'username': 'admin','password': '{password}'}}";
            HttpClient client = new HttpClient();
            var content = new StringContent(loginContet, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"http://{ip}:12800/login", content);
            var info = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(info);
            var token = obj["access_token"];

            Logger.Log($"Access Token get with {Convert.ToString(token).Substring(0, 10)}");

            var serviceClient = new HttpClient();
            string key;
            var weiboAnalysisPipeline = context.Pipeline as WeiboAnalysisPipeline;
            if (weiboAnalysisPipeline != null)
            {
                key = weiboAnalysisPipeline.WeiboContextKey;
                var retwittedContent = context[key];

                try
                {
                    int retryTimes = 0;
                    var insList = new List<KOL_pagerank>();
                    while (retryTimes <= 5)
                    {
                        try
                        {
                            client = new HttpClient();
                            var kolContent = $@"{{'fileStr': '{retwittedContent}'}}";
                            var messeage = new StringContent(kolContent, Encoding.UTF8, "application/json");
                            var request = new HttpRequestMessage
                            {
                                RequestUri = new Uri($"http://{ip}:12800/api/KOLService/v1.0.0"),
                                Method = HttpMethod.Post,
                                Content = messeage
                            };
                            request.Headers.Accept.Add(
                                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Add("Authorization", $"Bearer {Convert.ToString(token)}");
                            var kolResponse = await client.SendAsync(request);
                            var res = await kolResponse.Content.ReadAsStringAsync();
                            Logger.Log("KOL Response Get");
                            var answer = JObject.Parse(res)["outputParameters"]["answer"];
                            var items = JArray.Parse(answer.ToString());
                            insList.Clear();
                            foreach (var item in items)
                            {
                                var uid = Convert.ToString(item["uid"]);
                                var val = Convert.ToDouble(item["value"]);
                                var insKol = new KOL_pagerank
                                {
                                    uid = uid,
                                    value = val
                                };

                                insList.Add(insKol);
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                            retryTimes++;
                            Logger.Log($"Retry times {retryTimes}");
                        }
                    }

                    using (var db = ContextFactory.GetMediaAnalysisContext())
                    {
                        db.BulkInsert(insList);
                        db.SaveChanges();
                    }

                    Logger.Log("KOL Data Inserted");
                }
                catch (Exception e)
                {
                    Logger.Log(e);
                }
            }
            return result;
        }
    }
}

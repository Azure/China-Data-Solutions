using System.Collections.Generic;
using System.Diagnostics;

namespace NLPLib.Sentiment
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class SentimentAnalyzer
    {
        public async Task<SentimentResult> Analyze(string text)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://mcdisf.chinanorth.cloudapp.chinacloudapi.cn");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", text)
            });
            try
            {
                var response =
                    await client.PostAsync($"IntelligentService/SentimentService/api/sentiment/analyze", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<SentimentResult>();
                    result.Score = result.Score > 1 ? 1 : result.Score;
                    result.Score = result.Score < -1 ? -1 : result.Score;
                    return result;
                }
            }
            catch (Exception e)
            {
                ////@@TODO LOG
                Debug.WriteLine(e);
            }
            return null;
        }

        public async Task<List<KVPair<string, SentimentResult>>> BatchAnalyze(IEnumerable<KVPair<string, string>> textList)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://mcdisf.chinanorth.cloudapp.chinacloudapi.cn");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            BatchSentimentRequest request = new BatchSentimentRequest() { Body = textList.ToList() };
            var text = JsonConvert.SerializeObject(request);
            var content = new StringContent(text, Encoding.UTF8, "application/json");
            try
            {
                var response =
                    await client.PostAsync($"IntelligentService/SentimentService/api/sentiment/batchanalyze", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<KVPair<string, SentimentResult>>>();
                    foreach (var pair in result)
                    {
                        var item = pair.Value;
                        if (item != null)
                        {
                            item.Score = item.Score > 1 ? 1 : item.Score;
                            item.Score = item.Score < -1 ? -1 : item.Score;
                        }
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                ////@@TODO LOG
                Debug.WriteLine(e);
            }
            return null;
        }
    }
}
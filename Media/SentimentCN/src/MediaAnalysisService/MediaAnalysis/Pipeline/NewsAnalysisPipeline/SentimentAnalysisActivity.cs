using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NLPLib.KeywordExtract;
using NLPLib.Sentiment;
using System.Diagnostics;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class SentimentAnalysisActivity : IPipelineActivity
    {
        public ActivityResult Run(PipelineContext context)
        {
            var pipe = context.Pipeline as NewsAnalysisPipeline;
            ActivityResult result = new ActivityResult();
            IDictionary<long, SentimentResult> dict = new Dictionary<long, SentimentResult>();

            SentimentAnalyzer analyzer = new SentimentAnalyzer();
            if (pipe != null)
            {
                var newsList = context[pipe.NewsContextKey] as IEnumerable<NewsStream>;

                if (newsList != null)
                {
                    var textList = newsList.Select(i => new KVPair<string, string> { Key = i.Id.ToString(), Value = i.NewsArticleDescription }).ToList();
                    var sentiment = analyzer.BatchAnalyze(textList).Result;
                    for (var i = 0; i < textList.Count; i++)
                    {
                        var id = newsList.ElementAt(i).Id;
                        dict.Add(id, sentiment.FirstOrDefault(s => s.Key == id.ToString())?.Value);
                    }
                }
            }

            result.Result = dict;
            result.ObjectType = dict.GetType();
            return result;
        }

        public string Name { get; set; } = "SentimentAnalysis";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using NLPLib.KeywordExtract;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class ExtractKeywordsActivity : IPipelineActivity
    {
        public ActivityResult Run(PipelineContext context)
        {
            var pipe = context.Pipeline as NewsAnalysisPipeline;
            ActivityResult result = new ActivityResult();
            IDictionary<long, List<string>> dict = new Dictionary<long, List<string>>();

            KeywordExtractor extractor = new KeywordExtractor();
            var newsList = context[pipe.NewsContextKey] as IEnumerable<NewsStream>;

            if (newsList != null)
            {
                foreach (var news in newsList)
                {
                    var keywords = extractor.ExtractKeywordsWithTextRank(news.NewsArticleDescription).ToList();
                    dict.Add(news.Id, keywords ?? new List<string>());
                }
            }

            result.Result = dict;
            result.ObjectType = dict.GetType();
            return result;
        }

        public string Name { get; set; } = "ExtractKeyWord";
    }
}

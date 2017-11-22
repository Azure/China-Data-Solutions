using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.Pipeline;
using DataLibrary.Pipeline.WeiboAnalysis;
using DataLibrary.Pipeline.WeiboDataClean;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataPrepare
{
    public class WeiboAnalysisWorker
    {
        public async Task Run()
        {
            string retwittedContent;
            WeiboAnalysisPipeline pipe = new WeiboAnalysisPipeline();
            using (var db = ContextFactory.GetMediaAnalysisContext())
            {
                var list = db.Weibo_Retweeted.Select(i => new { from = i.id_from, to = i.id_to, weight = i.weight }).ToList();
                retwittedContent = JsonConvert.SerializeObject(list);
            }
            PipelineContext pctx = new PipelineContext { [pipe.WeiboContextKey] = retwittedContent };
            var result = pipe.Run(pctx).Result;
            if (!result.Succeeded)
            {
                throw result.Exception;
            }
            else
            {
                Logger.Log("KOL items processed.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.Models;
using DataLibrary.Pipeline;
using DataLibrary.Pipeline.WeiboDataClean;

namespace DataPrepare
{
    public class WeiboCleanWorker
    {
        public int Run()
        {
            var newsStreamList = this.QueryUnProcessedNews();
            var newsStreams = newsStreamList as Weibo_detailed[] ?? newsStreamList.ToArray();
            if (newsStreams.Any())
            {
                WeiboCleanPipeline pipe = new WeiboCleanPipeline();
                PipelineContext pctx = new PipelineContext { [pipe.WeiboContextKey] = newsStreamList };
                var result =  pipe.Run(pctx).Result;
                if (!result.Succeeded)
                {
                    throw result.Exception;
                }
                else
                {
                    Logger.Log($"{newsStreams.Count()} items processed.");
                }
            }
            return 0;
        }

        private IEnumerable<Weibo_detailed> QueryUnProcessedNews()
        {
            using (var context = ContextFactory.GetMediaAnalysisContext())
            {
                return
                    context.Weibo_detailed.Where(i => i.Processed != null && i.Processed == false || i.Processed == null).ToList();
            }
        }
    }
}

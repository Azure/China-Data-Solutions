using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using MediaAnalysis.Pipeline;
using MediaAnalysis.Pipeline.NewsAnalysisPipeline;

namespace NewsTextAnalysisJob
{
    public class NewsAnalysisWorker
    {
        public int Run()
        {
            var newsStreamList = this.QueryUnProcessedNews();
            var newsStreams = newsStreamList as NewsStream[] ?? newsStreamList.ToArray();
            if (newsStreams.Any())
            {
                NewsAnalysisPipeline pipe = new NewsAnalysisPipeline();
                PipelineContext pctx = new PipelineContext {[pipe.NewsContextKey] = newsStreamList};
                var result = pipe.Run(pctx);
                if (!result.Succeeded)
                {
                    throw result.Exception;
                }
            }
            else
            {
                System.Threading.Thread.Sleep(100000);
            }

            return newsStreams?.Count() ?? 0;
        }

        private IEnumerable<NewsStream> QueryUnProcessedNews()
        {
            using (var context = ContextFactory.GetMediaAnalysisContext())
            {
                return
                    context.NewsStreams.Where(i => i.KeyWords == null || i.KeyWords.Equals(string.Empty)).Take(200).ToList();
            }
        }
    }
}

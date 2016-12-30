using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using MediaAnalysis.Pipeline;
using MediaAnalysis.Pipeline.NewsAnalysisPipeline;

namespace MediaAnalysisRole
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
                pipe.Run(pctx);
            }

            return newsStreams?.Count() ?? 0;
        }

        private IEnumerable<NewsStream> QueryUnProcessedNews()
        {
            try
            {
                using (var context = ContextFactory.GetMediaAnalysisContext())
                {
                    return
                        context.NewsStreams.Where(i => i.KeyWords == null || i.KeyWords.Equals(string.Empty)).Take(200).ToList();
                }
            }
            catch (Exception ex)
            {
                ////@@TODO LOG
                Debug.WriteLine(ex);
            }

            return null;
        }
    }
}

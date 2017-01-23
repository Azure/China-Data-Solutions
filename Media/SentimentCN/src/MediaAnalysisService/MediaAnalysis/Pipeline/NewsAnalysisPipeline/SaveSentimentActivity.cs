using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using NLPLib.Sentiment;
using System.Diagnostics;
using EntityFramework.BulkInsert.Extensions;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class SaveSentimentActivity : IPipelineActivity
    {
        public string Name { get; set; } = "SaveSentimentResult";
        public ActivityResult Run(PipelineContext context)
        {
            var result = new ActivityResult();
            result.ObjectType = typeof(int);
            var obj = context.Result.ActivityResults["SentimentAnalysis"];
            var resultDict = Convert.ChangeType(obj.Result, obj.ObjectType) as IDictionary<long, SentimentResult> ;

            using (var db = ContextFactory.GetMediaAnalysisContext())
            {
                var list = new List<SentimentsResultNews>();
                foreach (var item in resultDict)
                {
                    var senti = new SentimentsResultNews
                    {
                        Id = item.Key,
                        Score =(decimal) Math.Round((double)item.Value.Score, 5),
                        Date = DateTime.UtcNow,
                        Name = string.Empty
                    };

                    list.Add(senti);
                }

                try
                {
                    db.BulkInsert(list);
                }
                catch (Exception e)
                {
                    //@@TODO LOG
                    Debug.WriteLine(e);
                    throw;
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Models;
using EntityFramework.BulkInsert.Extensions;

namespace DataLibrary.Pipeline.WeiboDataClean
{
    public class SaveRetweetedResultActivity : IPipelineActivity
    {
        public string Name { get; set; } = "SaveRetweetedResult";

        public Task<ActivityResult> Run(PipelineContext context)
        {
            var result = new ActivityResult();
            result.ObjectType = typeof(List<string>);
            var obj = context.Result.ActivityResults["ReweetAnalysis"];
            var resultList = Convert.ChangeType(obj.Result, obj.ObjectType) as List<Weibo_Retweeted>;
            var list = resultList.GroupBy(p => new { idf = p.id_from, idt = p.id_to}, (key, v) => new { Key= key, Value = v.Count() });

            var aggregateList = new List<Weibo_Retweeted>();
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Key.idf) && !string.IsNullOrEmpty(item.Key.idt))
                {
                    Weibo_Retweeted r = new Weibo_Retweeted
                    {
                        id_from = item.Key.idf,
                        id_to = item.Key.idt,
                        weight = item.Value
                    };
                    aggregateList.Add(r);
                }
            }

            var batch = 200;
            var totalBatches = aggregateList.Count / batch + 1;
            for (var i = 0; i < totalBatches; i++)
            {
                using (var db = ContextFactory.GetMediaAnalysisContext())
                {
                    var insertList = aggregateList.Skip(i * batch).Take(batch);
                    db.BulkInsert(insertList);
                    db.SaveChanges();
                }
            }

           
            return Task.FromResult(result);
        }
    }
}

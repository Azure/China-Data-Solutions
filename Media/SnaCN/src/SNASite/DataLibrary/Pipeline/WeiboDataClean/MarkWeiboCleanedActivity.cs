using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataLibrary.Models;

namespace DataLibrary.Pipeline.WeiboDataClean
{
    public class MarkWeiboCleanedActivity : IPipelineActivity
    {
        public string Name { get; set; }
        public ActivityResult Run(PipelineContext context)
        {
            var pipe = context.Pipeline as WeiboCleanPipeline;
            var result = new ActivityResult();
            result.ObjectType = typeof(int);
            var weibolist = context[pipe.WeiboContextKey] as IEnumerable<Weibo_detailed>;
            using (var db = ContextFactory.GetMediaAnalysisContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
                if (weibolist != null)
                {
                    foreach (var item in weibolist)
                    {
                        item.Processed = true;
                        db.Weibo_detailed.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                try
                {
                    result.Result = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ////@@TODO LOG
                    Debug.WriteLine(ex);
                }
            }

            return result;
        }
    }
}

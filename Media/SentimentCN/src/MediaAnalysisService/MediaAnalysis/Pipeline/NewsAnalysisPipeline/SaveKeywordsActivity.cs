﻿using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class SaveKeywordsActivity : IPipelineActivity
    {
        public string Name { get; set; } = "SaveKeywordsResult";
        public ActivityResult Run(PipelineContext context)
        {
            var pipe = context.Pipeline as NewsAnalysisPipeline;
            var result = new ActivityResult();
            result.ObjectType = typeof(int);
            var obj = context.Result.ActivityResults["ExtractKeyWord"];
            var resultDict = Convert.ChangeType(obj.Result, obj.ObjectType) as IDictionary<long, List<string>>;
            var newsList = context[pipe.NewsContextKey] as IEnumerable<NewsStream>;
            using (var db = ContextFactory.GetMediaAnalysisContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;
                foreach (var item in newsList)
                {
                    var keywords = resultDict.ContainsKey(item.Id) ? resultDict[item.Id] : null;
                    if (keywords.Any())
                    {
                        var keyword = string.Join(" ", keywords);
                        item.KeyWords = keyword;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Title))
                        {
                            var length = item.Title.Length > 20 ? 20 : item.Title.Length;
                            item.KeyWords = item.Title.Substring(0, length);
                        }
                        else
                        {
                            item.KeyWords = "Empty";
                        }
                    }

                    db.NewsStreams.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
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

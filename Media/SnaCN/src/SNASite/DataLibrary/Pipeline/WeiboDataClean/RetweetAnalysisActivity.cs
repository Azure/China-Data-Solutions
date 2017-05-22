using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Models;

namespace DataLibrary.Pipeline.WeiboDataClean
{
    public class RetweetAnalysisActivity : IPipelineActivity
    {
        public Task<ActivityResult> Run(PipelineContext context)
        {
            var pipe = context.Pipeline as WeiboCleanPipeline;
            ActivityResult result = new ActivityResult();

            List<Weibo_Retweeted> retweetedInfo = new List<Weibo_Retweeted>();

            var weibolist = context[pipe.WeiboContextKey] as IEnumerable<Weibo_detailed>;
            if (weibolist != null)
            {
                var retweetedList = weibolist.Where(i => !string.IsNullOrEmpty(i.retweeted_mid));
                foreach (var item in retweetedList)
                {
                    var line = new Weibo_Retweeted
                    {
                        mid = item.mid,
                        id_from = item.retweeted_uid,
                        id_to = item.user_uid,
                        weight = 1
                    };

                    retweetedInfo.Add(line);
                }
            }

            result.Result = retweetedInfo;
            result.ObjectType = retweetedInfo.GetType();
            return Task.FromResult(result);
        }

        public string Name { get; set; } = "ReweetAnalysis";
    }
}

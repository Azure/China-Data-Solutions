using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Pipeline.WeiboAnalysis
{
    public class WeiboAnalysisPipeline : PipelineBase
    {

        public WeiboAnalysisPipeline()
        {
            Init();
        }

        public override string Name { get; } = "WeiboAnalysis";

        public string WeiboContextKey { get; } = "WeiboRetweet";
        public override void Init()
        {
            this.Activities.Add(new KOLAnalysisActivity());
        }
    }
}

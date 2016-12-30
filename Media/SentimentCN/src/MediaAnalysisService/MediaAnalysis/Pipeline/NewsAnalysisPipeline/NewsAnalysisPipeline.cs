using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class NewsAnalysisPipeline : PipelineBase
    {
        public NewsAnalysisPipeline()
        {
            Init();
        }

        public override string Name { get; } = "NewsAnalysis";

        public string NewsContextKey { get; } = "News";

        public override void Init()
        {
            this.Activities.Add(new SentimentAnalysisActivity());
            this.Activities.Add(new ExtractKeywordsActivity());
            this.Activities.Add(new SaveSentimentActivity());
            this.Activities.Add(new SaveKeywordsActivity());
        }

    }
}

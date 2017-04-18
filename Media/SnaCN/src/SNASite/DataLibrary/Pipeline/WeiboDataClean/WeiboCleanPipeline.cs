namespace DataLibrary.Pipeline.WeiboDataClean
{
    public class WeiboCleanPipeline : PipelineBase
    {
        public WeiboCleanPipeline()
        {
            Init();
        }

        public override string Name { get; } = "WeiboClean";

        public string WeiboContextKey { get; } = "Weibo";

        public override void Init()
        {
            this.Activities.Add(new RetweetAnalysisActivity());
            this.Activities.Add(new SaveRetweetedResultActivity());
        }
    }
}

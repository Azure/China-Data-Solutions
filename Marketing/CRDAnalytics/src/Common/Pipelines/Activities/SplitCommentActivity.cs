namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    using Models;

    public sealed class SplitCommentActivity : ActivityBase<CustomerReviewModel, CustomerReviewSentencesModel>
    {
        private const string SentenceRegexGroupName = @"sentence";

        private static readonly Regex SentenceRegex =
            new Regex($"(?<{SentenceRegexGroupName}>[^。！？]+[。！？]*)", RegexOptions.Compiled);

        public SplitCommentActivity(string activityType = null)
            : base(activityType)
        {
        }

        protected override Task<CustomerReviewSentencesModel> ProcessModelAsync(ActivityContext activityContext)
        {
            var inputModel = activityContext.GetInputModel<CustomerReviewModel>();

            var sentences =
              SentenceRegex.Matches(inputModel.Comment)
                  .Cast<Match>()
                  .Select(m => m.Groups[SentenceRegexGroupName].Value)
                  .ToList();

            return Task.FromResult(new CustomerReviewSentencesModel(inputModel, sentences));
        }
    }
}

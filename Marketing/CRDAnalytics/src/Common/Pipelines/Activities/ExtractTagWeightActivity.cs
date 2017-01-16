namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;
    using Nlp.TagWeight;

    public sealed class ExtractTagWeightActivity : ActivityBase<CustomerReviewSentencesModel, CustomerReviewSentencesTagWeightModel>
    {
        public ExtractTagWeightActivity(string activityType = null)
            : base(activityType)
        {
        }

        protected override Task<CustomerReviewSentencesTagWeightModel> ProcessModelAsync(ActivityContext activityContext)
        {
            var inputModel = activityContext.GetInputModel<CustomerReviewSentencesModel>();

            var sentences = inputModel.Sentences as IList<string> ?? inputModel.Sentences.ToList();

            var sentenceTagWeightResults = new List<SentenceTagWeightResult>();

            for (var sentenceIndex = 0; sentenceIndex < sentences.Count; sentenceIndex++)
            {
                var sentence = sentences[sentenceIndex];

                var tagWeightResult = TagWeightService.Extract(sentence);

                sentenceTagWeightResults.AddRange(
                    tagWeightResult.Select(r =>
                        new SentenceTagWeightResult(sentenceIndex, sentence, r.Word, r.Weight)));
            }

            return Task.FromResult(
                new CustomerReviewSentencesTagWeightModel(inputModel.CustomerReviewModel, sentenceTagWeightResults));
        }
    }
}

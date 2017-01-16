namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;
    using Nlp.Sentiment;

    public sealed class AnalyzeSentimentActivity
        : ActivityBase<CustomerReviewSentencesModel, CustomerReviewSentencesSentimentModel>
    {
        public AnalyzeSentimentActivity(string activityType = null)
            : base(activityType)
        {
        }

        protected override async Task<CustomerReviewSentencesSentimentModel> ProcessModelAsync(
            ActivityContext activityContext)
        {
            var inputModel = activityContext.GetInputModel<CustomerReviewSentencesModel>();

            var sentences = inputModel.Sentences as IList<string> ?? inputModel.Sentences.ToList();

            var sentenceSentimentResults = new List<SentenceSentimentResult>();

            for (var sentenceIndex = 0; sentenceIndex < sentences.Count; sentenceIndex++)
            {
                var sentence = sentences[sentenceIndex];

                var sentimentResult = await SentimentClient.AnalyzeAsync(sentence);

                sentenceSentimentResults.Add(
                    new SentenceSentimentResult(
                        sentenceIndex,
                        sentence,
                        sentimentResult.SentimentType.ToString("G"),
                        sentimentResult.Score));
            }

            return new CustomerReviewSentencesSentimentModel(
                inputModel.CustomerReviewModel,
                sentenceSentimentResults);
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Extensions;
    using Models;
    using Nlp.Sentiment;

    /// <summary>
    /// Defines the analyze sentiment activity class.
    /// </summary>
    /// <seealso cref="ActivityBase{CustomerReviewSentencesModel, CustomerReviewSentencesSentimentModel}" />
    public sealed class AnalyzeSentimentActivity
        : ActivityBase<CustomerReviewSentencesModel, CustomerReviewSentencesSentimentModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeSentimentActivity"/> class.
        /// </summary>
        /// <param name="activityType">Type of the activity.</param>
        public AnalyzeSentimentActivity(string activityType = null)
            : base(activityType)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The output model.
        /// </returns>
        protected override async Task<CustomerReviewSentencesSentimentModel> ProcessModelAsync(
            ActivityContext activityContext)
        {
            ////Trace.TraceInformation(
            ////    $"Analyzing sentiment: {activityContext.InputModel.CorrelationId}");

            try
            {
                var inputModel = activityContext.GetInputModel<CustomerReviewSentencesModel>();

                var sentences = inputModel.Sentences as IList<string> ?? inputModel.Sentences.ToList();

                var textDictionary =
                    sentences.Select((s, idx) => new KeyValuePair<int, string>(idx, s))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                var sentimentResult = await SentimentClient.BatchAnalyzeAsync(textDictionary);

                var sentenceSentimentResults =
                    sentimentResult.Select(
                        kvp =>
                            new SentenceSentimentResult(
                                kvp.Key,
                                textDictionary[kvp.Key],
                                kvp.Value.SentimentType.ToString("G"),
                                kvp.Value.Score));

                return new CustomerReviewSentencesSentimentModel(
                    inputModel.CustomerReviewModel,
                    sentenceSentimentResults);
            }
            catch (Exception ex)
            {
                Trace.TraceWarning(
                    $"Analyze sentiment failed, exception detail: {ex.GetDetailMessage()}, context: {activityContext.ToJsonIndented()}");

                throw;
            }
        }

        #endregion
    }
}

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
    using Nlp.TagWeight;

    /// <summary>
    /// Defines the extract tag weight activity class.
    /// </summary>
    /// <seealso cref="ActivityBase{CustomerReviewSentencesModel, CustomerReviewSentencesTagWeightModel}" />
    public sealed class ExtractTagWeightActivity
        : ActivityBase<CustomerReviewSentencesModel, CustomerReviewSentencesTagWeightModel>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractTagWeightActivity"/> class.
        /// </summary>
        /// <param name="activityType">Type of the activity.</param>
        public ExtractTagWeightActivity(string activityType = null)
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
        protected override Task<CustomerReviewSentencesTagWeightModel> ProcessModelAsync(ActivityContext activityContext)
        {
            ////Trace.TraceInformation(
            ////    $"Extracting tag weight: {activityContext.InputModel.CorrelationId}");

            try
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
            catch (Exception ex)
            {
                Trace.TraceWarning(
                                   $"Extracting tag weight failed, exception detail: {ex.GetDetailMessage()}, context: {activityContext.ToJsonIndented()}");

                throw;
            }
        }

        #endregion
    }
}

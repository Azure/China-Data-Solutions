// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Models;

    /// <summary>
    /// Defines the split comment activity class.
    /// </summary>
    /// <seealso cref="ActivityBase{CustomerReviewModel, CustomerReviewSentencesModel}" />
    public sealed class SplitCommentActivity : ActivityBase<CustomerReviewModel, CustomerReviewSentencesModel>
    {
        #region Fields

        /// <summary>
        /// The sentence regular expression group name
        /// </summary>
        private const string SentenceRegexGroupName = @"sentence";

        /// <summary>
        /// The sentence regular expression
        /// </summary>
        private static readonly Regex SentenceRegex =
            new Regex($"(?<{SentenceRegexGroupName}>[^。！？]+[。！？]*)", RegexOptions.Compiled);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitCommentActivity"/> class.
        /// </summary>
        /// <param name="activityType">Type of the activity.</param>
        public SplitCommentActivity(string activityType = null)
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

        #endregion
    }
}

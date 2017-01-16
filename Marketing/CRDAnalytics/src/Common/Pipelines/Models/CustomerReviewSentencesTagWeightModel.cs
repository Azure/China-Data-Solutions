namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the customer review sentence tag weight model class.
    /// </summary>
    /// <seealso cref="DataModelBase" />
    [Serializable]
    public sealed class CustomerReviewSentencesTagWeightModel : DataModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewSentencesTagWeightModel" /> class.
        /// </summary>
        /// <param name="customerReviewModel">The customer review model.</param>
        /// <param name="tagWeightResults">The tag weight result.</param>
        public CustomerReviewSentencesTagWeightModel(
            CustomerReviewModel customerReviewModel,
            IEnumerable<SentenceTagWeightResult> tagWeightResults)
            : base(customerReviewModel.CorrelationId)
        {
            this.CustomerReviewModel = customerReviewModel;
            this.TagWeightResults = tagWeightResults;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the customer review model.
        /// </summary>
        /// <value>
        /// The customer review model.
        /// </value>
        public CustomerReviewModel CustomerReviewModel { get; }

        /// <summary>
        /// Gets the tag weight result.
        /// </summary>
        /// <value>
        /// The tag weight result.
        /// </value>
        public IEnumerable<SentenceTagWeightResult> TagWeightResults { get; }

        #endregion
    }
}

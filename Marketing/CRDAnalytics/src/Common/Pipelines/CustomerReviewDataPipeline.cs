// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using Activities;
    using DataProviders;

    /// <summary>
    /// Defines the customer review data pipeline.
    /// </summary>
    /// <seealso cref="PipelineBase" />
    public sealed class CustomerReviewDataPipeline : PipelineBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewDataPipeline"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="activityHub">The activity hub.</param>
        /// <param name="pipelineType">Type of the pipeline.</param>
        public CustomerReviewDataPipeline(
            IDataProvider dataProvider,
            IActivityHub activityHub,
            string pipelineType = null)
            : base(dataProvider, activityHub, pipelineType)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the action.
        /// </summary>
        protected override void ExecuteAction()
        {
            var dataProviderResult = this.DataProvider.GetModels();

            this.ProcessDataProviderResult(dataProviderResult);
        }

        /// <summary>
        /// Processes the data provider result.
        /// </summary>
        /// <param name="dataProviderResult">The data provider result.</param>
        private void ProcessDataProviderResult(DataProviderResult dataProviderResult)
        {
            var currentResult = dataProviderResult;

            while (currentResult != null)
            {
                if (currentResult.HasModels)
                {
                    this.ActivityHub.ProcessModels(currentResult.ModelType, currentResult.Models);
                }
                else
                {
                    break;
                }

                if (!currentResult.HasMoreModels)
                {
                    break;
                }

                var pagingInfo = currentResult.PagingInfo;
                pagingInfo.PageIndex++;

                currentResult = this.DataProvider.GetModels(pagingInfo);
            }
        }

        #endregion
    }
}

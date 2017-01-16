using System;
using System.Threading;

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using Activities;
    using DataProviders;

    public sealed class CustomerReviewDataPipeline : PipelineBase
    {
        public CustomerReviewDataPipeline(
            IDataProvider dataProvider,
            IActivityHub activityHub,
            string pipelineType = null)
            : base(dataProvider, activityHub, pipelineType)
        {
        }

        protected override void ExecuteAction()
        {
            var dataProviderResult = this.DataProvider.GetModels();

            this.ProcessDataProviderResult(dataProviderResult);

            while (this.ActivityHub.RunningTasksCount > 0)
            {
                Console.WriteLine($"Running {this.ActivityHub.RunningTasksCount} tasks.");
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }

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
    }
}

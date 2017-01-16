namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public sealed class DefaultActivityHub : ActivityHubBase
    {
        public DefaultActivityHub(IEnumerable<IActivity> activities)
            : base(activities)
        {
        }

        public override void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels)
        {
            Parallel.ForEach(inputModels, new ParallelOptions {MaxDegreeOfParallelism = 4}, async inputModel =>
            {
                await this.ProcessModelAsync(modelType, inputModel);
            });
        }

        public override async Task ProcessModelAsync(Type modelType, IDataModel inputModel)
        {
            foreach (var activity in this.Activities.Where(a => a.CanProcess(modelType)))
            {
                await activity.ProcessModelAsync(inputModel);
            }
        }

        protected override async void AcvitityExecutedEventHandler(IActivity sender, ActivityExecutedEventArgs args)
        {
            base.AcvitityExecutedEventHandler(sender, args);

            var activityResult = args.Context.Result;
            if (activityResult.IsSuccess && !activityResult.Value.IsEmptyModel())
            {
                await this.ProcessModelAsync(sender.Metadata.OutputModelType, activityResult.Value);
            }
        }
    }
}

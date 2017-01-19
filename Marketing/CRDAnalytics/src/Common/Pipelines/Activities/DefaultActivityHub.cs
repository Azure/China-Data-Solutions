// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Extensions;
    using Models;

    /// <summary>
    /// Defines the default activity hub.
    /// </summary>
    /// <seealso cref="ActivityHubBase" />
    public sealed class DefaultActivityHub : ActivityHubBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultActivityHub"/> class.
        /// </summary>
        /// <param name="activities">The activities.</param>
        public DefaultActivityHub(IEnumerable<IActivity> activities)
            : base(activities)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the models.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModels">The input models.</param>
        public override void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels)
        {
            foreach (var inputModel in inputModels)
            {
                this.ProcessModel(modelType, inputModel);
            }
        }

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModel">The input model.</param>
        public override void ProcessModel(Type modelType, IDataModel inputModel)
        {
            foreach (var activity in this.Activities.Where(a => a.CanProcess(modelType)))
            {
                var activityResult = activity.ProcessModel(inputModel);

                if (activityResult.IsSuccess)
                {
                    if (!activityResult.Value.IsEmptyModel())
                    {
                        this.ProcessModel(activity.Metadata.OutputModelType, activityResult.Value);
                    }
                }
                else
                {
                    Trace.TraceWarning($"Activity executed failed, exception detail: {activityResult.Exception.GetDetailMessage()}, input model: {inputModel.ToJsonIndented()}");
                }
            }
        }

        #endregion
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    /// <summary>
    /// Defines the extension methods for data models.
    /// </summary>
    public static class DataModelExtensiosn
    {
        #region Methods

        /// <summary>
        /// Determines whether [is empty model].
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <returns>
        ///   <c>true</c> if [is empty model] [the specified data model]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmptyModel(this IDataModel dataModel) => dataModel is EmptyModel;

        #endregion
    }
}

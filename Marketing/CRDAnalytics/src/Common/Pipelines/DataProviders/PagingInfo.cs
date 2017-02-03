// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    /// <summary>
    /// Defines the paging info class.
    /// </summary>
    public sealed class PagingInfo
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingInfo"/> class.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        public PagingInfo(int totalCount, int pageSize)
        {
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.PageIndex = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        public int PageCount => this.TotalCount < this.PageSize ? 1 : (this.TotalCount / this.PageSize);

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastPage => this.PageIndex == (this.PageCount - 1);

        #endregion
    }
}

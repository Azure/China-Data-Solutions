// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Nlp.TagWeight
{
    /// <summary>
    /// Defines the tag weight result.
    /// </summary>
    internal sealed class TagWeightResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TagWeightResult"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="weight">The weight.</param>
        public TagWeightResult(string word, double weight)
        {
            this.Word = word;
            this.Weight = weight;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tag word.
        /// </summary>
        /// <value>
        /// The tag word.
        /// </value>
        public string Word { get; }

        /// <summary>
        /// Gets the tag weight.
        /// </summary>
        /// <value>
        /// The tag weight.
        /// </value>
        public double Weight { get; }

        #endregion
    }
}

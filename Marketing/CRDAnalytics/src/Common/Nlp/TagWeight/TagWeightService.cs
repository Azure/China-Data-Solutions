// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Nlp.TagWeight
{
    using System.Collections.Generic;
    using System.Linq;

    using JiebaNet.Analyser;

    /// <summary>
    /// Defines the tag weight service.
    /// </summary>
    internal static class TagWeightService
    {
        #region Fields

        /// <summary>
        /// The tag extractor
        /// </summary>
        private static readonly TextRankExtractor TagExtractor = new TextRankExtractor();

        #endregion

        #region Methods

        /// <summary>
        /// Extracts the tag and weight results from specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The extracted tag and weight results.</returns>
        public static IEnumerable<TagWeightResult> Extract(string text) =>
            TagExtractor.ExtractTagsWithWeight(text).Select(t => new TagWeightResult(t.Word, t.Weight));

        #endregion
    }
}

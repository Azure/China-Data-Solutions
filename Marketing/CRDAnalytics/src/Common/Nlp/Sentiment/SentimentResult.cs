// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Nlp.Sentiment
{
    using Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Defines the sentiment service result.
    /// </summary>
    public sealed class SentimentResult
    {
        #region Properties

        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        /// <value>
        /// The input string.
        /// </value>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the type of the sentiment.
        /// </summary>
        /// <value>
        /// The type of the sentiment.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public Polarity SentimentType { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public double Score { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.ToJsonIndented();

        #endregion
    }
}

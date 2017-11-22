// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    /// <summary>
    /// Defines the sentence sentiment result class.
    /// </summary>
    [Serializable]
    public sealed class SentenceSentimentResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SentenceSentimentResult"/> class.
        /// </summary>
        /// <param name="sentenceIndex">Index of the sentence.</param>
        /// <param name="sentence">The sentence.</param>
        /// <param name="polarity">The polarity.</param>
        /// <param name="sentimentScore">The sentiment score.</param>
        public SentenceSentimentResult(int sentenceIndex, string sentence, string polarity, double sentimentScore)
        {
            this.SentenceIndex = sentenceIndex;
            this.Sentence = sentence;
            this.Polarity = polarity;
            this.SentimentScore = sentimentScore;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index of the sentence.
        /// </summary>
        /// <value>
        /// The index of the sentence.
        /// </value>
        public int SentenceIndex { get; }

        /// <summary>
        /// Gets the sentence.
        /// </summary>
        /// <value>
        /// The sentence.
        /// </value>
        public string Sentence { get; }

        /// <summary>
        /// Gets the polarity.
        /// </summary>
        /// <value>
        /// The polarity.
        /// </value>
        public string Polarity { get; }

        /// <summary>
        /// Gets the sentiment score.
        /// </summary>
        /// <value>
        /// The sentiment score.
        /// </value>
        public double SentimentScore { get; }

        #endregion
    }
}
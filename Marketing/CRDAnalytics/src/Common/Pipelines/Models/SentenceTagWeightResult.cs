// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    /// <summary>
    /// Defines the sentence tag weight result class.
    /// </summary>
    [Serializable]
    public sealed class SentenceTagWeightResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SentenceTagWeightResult" /> class.
        /// </summary>
        /// <param name="sentenceIndex">Index of the sentence.</param>
        /// <param name="sentence">The sentence.</param>
        /// <param name="word">The word.</param>
        /// <param name="weight">The weight.</param>
        public SentenceTagWeightResult(int sentenceIndex, string sentence, string word, double weight)
        {
            this.Word = word;
            this.Weight = weight;
            this.SentenceIndex = sentenceIndex;
            this.Sentence = sentence;
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
        /// Gets the word.
        /// </summary>
        /// <value>
        /// The word.
        /// </value>
        public string Word { get; }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; }

        #endregion
    }
}

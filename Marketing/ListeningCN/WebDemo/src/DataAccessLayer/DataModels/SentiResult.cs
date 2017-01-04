// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="SentiResult.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using MSRA.NLC.Sentiment.Common;

    /// <summary>
    /// Class SentiResult.
    /// </summary>
    public class SentiResult
    {
        /// <summary>
        /// Gets or sets the input string.
        /// </summary>
        /// <value>The input string.</value>
        public string InputString { get; set; }

        /// <summary>
        /// Gets or sets the type of the sentiment.
        /// </summary>
        /// <value>The type of the sentiment.</value>
        public Polarity SentimentType { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public double Score { get; set; }
    }
}
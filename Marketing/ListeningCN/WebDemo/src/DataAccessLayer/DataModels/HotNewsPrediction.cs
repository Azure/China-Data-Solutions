// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="HotNewsPrediction.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class HotNewsPrediction.
    /// </summary>
    public class HotNewsPrediction
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the cluster id0.
        /// </summary>
        /// <value>The cluster id0.</value>
        public string ClusterId0 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id4.
        /// </summary>
        /// <value>The cluster id4.</value>
        public string ClusterId4 { get; set; }

        /// <summary>
        /// Gets or sets the prediction time.
        /// </summary>
        /// <value>The prediction time.</value>
        public DateTime? PredictionTime { get; set; }

        /// <summary>
        /// Gets or sets the probability.
        /// </summary>
        /// <value>The probability.</value>
        public decimal? Probability { get; set; }

        /// <summary>
        /// Gets or sets the hot rank.
        /// </summary>
        /// <value>The hot rank.</value>
        public int? HotRank { get; set; }
    }
}
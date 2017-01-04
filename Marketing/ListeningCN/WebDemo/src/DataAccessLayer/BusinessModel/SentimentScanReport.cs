// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="SentimentScanReport.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    /// <summary>
    /// Class SentimentScanReport.
    /// </summary>
    public class SentimentScanReport
    {
        /// <summary>
        /// Gets or sets the negative perct.
        /// </summary>
        /// <value>The negative perct.</value>
        public double NegativePerct { get; set; }

        /// <summary>
        /// Gets or sets the positive perct.
        /// </summary>
        /// <value>The positive perct.</value>
        public double PositivePerct { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public decimal Score { get; set; }
    }
}
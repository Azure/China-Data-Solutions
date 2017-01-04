// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="SentimentsResult.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class SentimentsResult.
    /// </summary>
    public class SentimentsResult
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the positive.
        /// </summary>
        /// <value>The positive.</value>
        public int Positive { get; set; }

        /// <summary>
        /// Gets or sets the negative.
        /// </summary>
        /// <value>The negative.</value>
        public int Negative { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the maximum identifier.
        /// </summary>
        /// <value>The maximum identifier.</value>
        public long? MaxId { get; set; }
    }
}
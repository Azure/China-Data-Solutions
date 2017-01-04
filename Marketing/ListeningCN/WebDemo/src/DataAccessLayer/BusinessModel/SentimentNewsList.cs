// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="SentimentNewsList.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class SentimentNewsList.
    /// </summary>
    public class SentimentNewsList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SentimentNewsList"/> class.
        /// </summary>
        public SentimentNewsList()
        {
            this.PositiveNewsList = new List<NewsBrief>();
            this.NegativeNewsList = new List<NewsBrief>();
        }

        /// <summary>
        /// Gets or sets the positive news list.
        /// </summary>
        /// <value>The positive news list.</value>
        public List<NewsBrief> PositiveNewsList { get; set; }

        /// <summary>
        /// Gets or sets the negative news list.
        /// </summary>
        /// <value>The negative news list.</value>
        public List<NewsBrief> NegativeNewsList { get; set; }
    }
}
// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CommentsSentiments.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer
{
    using System;

    /// <summary>
    /// Class CommentsSentiments.
    /// </summary>
    public class CommentsSentiments
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the attitute.
        /// </summary>
        /// <value>The attitute.</value>
        public string Attitute { get; set; }

        /// <summary>
        /// Gets or sets the vote.
        /// </summary>
        /// <value>The vote.</value>
        public long Vote { get; set; }
    }
}
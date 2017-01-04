// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="KeywordsModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class KeywordsModel.
    /// </summary>
    public class KeywordsModel
    {
        /// <summary>
        /// Gets or sets the compnay keyword.
        /// </summary>
        /// <value>The compnay keyword.</value>
        public string CompnayKeyword { get; set; }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        public List<string> Keywords { get; } = new List<string>();
    }
}
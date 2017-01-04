// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WordCloudResult.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class WordCloudResult.
    /// </summary>
    public class WordCloudResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordCloudResult"/> class.
        /// </summary>
        public WordCloudResult()
        {
            this.WordDic = new Dictionary<string, WordStatistic>();
            this.NewsList = new List<NewsBrief>();
        }

        /// <summary>
        /// Gets or sets the word dic.
        /// </summary>
        /// <value>The word dic.</value>
        public Dictionary<string, WordStatistic> WordDic { get; set; }

        /// <summary>
        /// Gets or sets the news list.
        /// </summary>
        /// <value>The news list.</value>
        public List<NewsBrief> NewsList { get; set; }

        /// <summary>
        /// Gets or sets the key word.
        /// </summary>
        /// <value>The key word.</value>
        public string KeyWord { get; set; }
    }
}
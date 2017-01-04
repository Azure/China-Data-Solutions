// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboLastProcessRecord.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    /// <summary>
    /// Class WeiboLastProcessRecord.
    /// </summary>
    public class WeiboLastProcessRecord
    {
        /// <summary>
        /// Gets or sets the source predict identifier.
        /// </summary>
        /// <value>The source predict identifier.</value>
        public long SourcePredictId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }
    }
}
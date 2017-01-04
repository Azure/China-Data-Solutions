// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ClientUserProfile.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class ClientUserProfile.
    /// </summary>
    public class ClientUserProfile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the postfix.
        /// </summary>
        /// <value>The postfix.</value>
        public string Postfix { get; set; }

        /// <summary>
        /// Gets or sets the competitor filters.
        /// </summary>
        /// <value>The competitor filters.</value>
        public string CompetitorFilters { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public string Filters { get; set; }

        /// <summary>
        /// Gets or sets the last updated time.
        /// </summary>
        /// <value>The last updated time.</value>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>The created time.</value>
        public DateTime CreatedTime { get; set; }
    }
}
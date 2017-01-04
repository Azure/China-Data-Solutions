// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-19-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CustomerFilters.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Filters
{
    using System.Collections.Generic;

    /// <summary>
    /// Class CustomerFilters.
    /// </summary>
    public class CustomerFilters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFilters"/> class.
        /// </summary>
        public CustomerFilters()
        {
            this.UserFilterListCollection = new List<FilterList>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerFilters"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public CustomerFilters(string userName)
            : this()
        {
            this.UserName = userName;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user filter list collection.
        /// </summary>
        /// <value>The user filter list collection.</value>
        public List<FilterList> UserFilterListCollection { get; set; }
    }

    /// <summary>
    /// Class CustomerFilterResult.
    /// </summary>
    public class CustomerFilterResult
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }
    }
}
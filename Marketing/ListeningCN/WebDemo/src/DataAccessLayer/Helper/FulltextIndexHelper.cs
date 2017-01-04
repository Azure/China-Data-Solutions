// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-08-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="FulltextIndexHelper.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Helper
{
    using System.Linq;

    using DataAccessLayer.DataModels.Filters;

    /// <summary>
    /// Class FulltextIndexHelper.
    /// </summary>
    public class FulltextIndexHelper
    {
        /// <summary>
        /// Builds the pattern from filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>System.String.</returns>
        public string BuildPatternFromFilter(CustomerFilters filter)
        {
            var result = string.Join(" or ", filter.UserFilterListCollection.Select(this.BuildPatternForFilterList));
            return result;
        }

        /// <summary>
        /// Builds the pattern for filter list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>System.String.</returns>
        private string BuildPatternForFilterList(FilterList list)
        {
            return string.Join(" and ", list.Filters.Select(i => i.Value));
        }
    }
}
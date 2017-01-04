// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="FilterList.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Filters
{
    using System.Collections.Generic;

    /// <summary>
    /// Class FilterList.
    /// </summary>
    public class FilterList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        public FilterList()
        {
            this.Filters = new List<Filter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        /// <param name="logicOperator">The logic operator.</param>
        public FilterList(string logicOperator)
            : this()
        {
            this.LogicOperator = logicOperator;
        }

        /// <summary>
        /// Gets or sets the logic operator.
        /// </summary>
        /// <value>The logic operator.</value>
        public string LogicOperator { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public List<Filter> Filters { get; set; }
    }
}
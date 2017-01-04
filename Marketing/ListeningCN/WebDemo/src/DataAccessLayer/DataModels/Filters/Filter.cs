// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="Filter.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Filters
{
    /// <summary>
    /// Class Filter.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Could be the target column need to use filters.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// And/Or the logic operator
        /// </summary>
        /// <value>The logic operator.</value>
        public string LogicOperator { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public string Operator { get; set; }
    }
}
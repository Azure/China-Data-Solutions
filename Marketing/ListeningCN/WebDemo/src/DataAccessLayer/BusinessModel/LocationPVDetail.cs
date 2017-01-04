// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="LocationPVDetail.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    /// <summary>
    /// Class LocationPVDetail.
    /// </summary>
    public class LocationPVDetail
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the visit count.
        /// </summary>
        /// <value>The visit count.</value>
        public int VisitCount { get; set; }

        /// <summary>
        /// Gets or sets the report count.
        /// </summary>
        /// <value>The report count.</value>
        public int ReportCount { get; set; }
    }
}
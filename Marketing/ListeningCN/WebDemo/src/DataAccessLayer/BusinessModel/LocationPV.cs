// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-29-2016
// ***********************************************************************
// <copyright file="LocationPV.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class LocationPV.
    /// </summary>
    public class LocationPV
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public List<LocationPVDetail> Details { get; } = new List<LocationPVDetail>();
    }
}
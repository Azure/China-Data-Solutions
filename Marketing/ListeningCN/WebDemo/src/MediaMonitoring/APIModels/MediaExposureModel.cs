// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="MediaExposureModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Class MediaExposureModel.
    /// </summary>
    public class MediaExposureModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the report count.
        /// </summary>
        /// <value>The report count.</value>
        public Dictionary<string, int> ReportCount { get; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets the visit count.
        /// </summary>
        /// <value>The visit count.</value>
        public Dictionary<string, int> VisitCount { get; } = new Dictionary<string, int>();
    }
}
// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="MediaExposureDetail.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    /// <summary>
    /// Class MediaExposureDetail.
    /// </summary>
    public class MediaExposureDetail
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }

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
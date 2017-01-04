// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="MediaExposureListModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Class MediaExposureListModel.
    /// </summary>
    public class MediaExposureListModel
    {
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <value>The list.</value>
        public List<MediaExposureModel> List { get; } = new List<MediaExposureModel>();
    }
}
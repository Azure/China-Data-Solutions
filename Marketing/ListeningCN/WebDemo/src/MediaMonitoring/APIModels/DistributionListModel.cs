// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="DistributionListModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Class DistributionListModel.
    /// </summary>
    public class DistributionListModel
    {
        /// <summary>
        /// Gets the distributions.
        /// </summary>
        /// <value>The distributions.</value>
        public List<DistributionModel> Distributions { get; } = new List<DistributionModel>();
    }
}
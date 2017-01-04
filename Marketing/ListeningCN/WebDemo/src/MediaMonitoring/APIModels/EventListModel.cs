// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="EventListModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Class EventListModel.
    /// </summary>
    public class EventListModel
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        public List<EventModel> Events { get; } = new List<EventModel>();
    }
}
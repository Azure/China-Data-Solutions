// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 08-09-2016
//
// Last Modified By : 
// Last Modified On : 08-22-2016
// ***********************************************************************
// <copyright file="ResponseModelConverter.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataAccessLayer.BusinessModel;

    using MediaMonitoring.APIModels;

    using EventDetail = MediaMonitoring.APIModels.EventDetail;

    /// <summary>
    /// Class ResponseModelConverter.
    /// </summary>
    public static class ResponseModelConverter
    {
        /// <summary>
        /// Converts the event model.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>EventListModel.</returns>
        public static EventListModel ConvertEventModel(List<Event> events)
        {
            var model = new EventListModel();
            if (events != null&&events.Count>0)
            {
                foreach (var item in events)
                {
                    var em = new EventModel
                                 {
                                     Name = item.Title,
                                     Sentiment = item.Sentiment,
                                     Company = item.Company,
                                     Keywords = item.Keywords,
                                     Url = item.Url,
                                     Source = item.Source,
                                     Date = item.Date
                                 };
                    foreach (var c in item.Comments)
                    {
                        em.Comments.Add(c);
                    }

                    if (item.EventDetails.Count > 0)
                    {
                        var minDate = item.EventDetails.Select(i => Convert.ToDateTime(i.Date)).Min();
                        em.DailyEvents.Add(
                            new EventDetail { Time = minDate.AddDays(-1).ToShortDateString(), Value = 0, Text = "0" });

                        foreach (var d in item.EventDetails)
                        {
                            var detail = new EventDetail
                                             {
                                                 Time = d.Date,
                                                 Value = d.VisitCount,
                                                 Text = d.VisitCount.ToString()
                                             };
                            em.DailyEvents.Add(detail);
                        }

                        var maxDate = item.EventDetails.Select(i => Convert.ToDateTime(i.Date)).Max();
                        em.DailyEvents.Add(
                            new EventDetail { Time = maxDate.AddDays(1).ToShortDateString(), Value = 0, Text = "0" });
                    }

                    model.Events.Add(em);
                }
            }

            return model;
        }
    }
}
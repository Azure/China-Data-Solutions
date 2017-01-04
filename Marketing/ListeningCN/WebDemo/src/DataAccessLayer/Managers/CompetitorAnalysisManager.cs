// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CompetitorAnalysisManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataAccessLayer.BusinessModel;

    /// <summary>
    /// The competitor analysis manager.
    /// </summary>
    public class CompetitorAnalysisManager
    {
        /// <summary>
        /// The cadata manager
        /// </summary>
        private readonly CADataManager cadataManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitorAnalysisManager" /> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="clientUser">The client user.</param>
        public CompetitorAnalysisManager(SysConfig config, ClientUser clientUser)
        {
            this.cadataManager = new CADataManager(config, clientUser);
        }

        /// <summary>
        /// The get max date.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The <see cref="DateTime" />.</returns>
        public DateTime GetMaxDate(string dataType)
        {
            return this.cadataManager.GetMaxDate(dataType);
        }

        /// <summary>
        /// The get event river data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<Event> GetEventRiverData(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.EVENT, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<Event>>(data);
            }

            return null;
        }

        /// <summary>
        /// The get media exposure.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<MediaExposure> GetMediaExposure(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.MEDIA, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<MediaExposure>>(data);
            }

            return null;
        }

        /// <summary>
        /// The get location distr.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<LocationPV> GetLocationDistr(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.LOCATION, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<LocationPV>>(data);
            }

            return null;
        }

        /// <summary>
        /// The get sentiments data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<SentimentResult> GetSentimentsData(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.SENTIMENTS, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<SentimentResult>>(data);
            }

            return null;
        }

        /// <summary>
        /// The get age data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<AgeDistribution> GetAgeData(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.AGE, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<AgeDistribution>>(data);
            }

            return null;
        }

        /// <summary>
        /// The get top competitor news.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<Event> GetTopCompetitorNews(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.TOPNEWS, date);

            if (data != null)
            {
                var model = AnalysisDataConvert.FromCAData<List<Event>>(data);
                return model.OrderByDescending(i => i.Date).ToList();
            }

            return null;
        }

        /// <summary>
        /// The get most sentiment news.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<Event> GetMostSentimentNews(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.TOPSENTINEWS, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<Event>>(data);
            }

            return null;
        }
    }
}
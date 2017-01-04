// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeeklyReportModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class TimeCount.
    /// </summary>
    public class TimeCount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCount"/> class.
        /// </summary>
        public TimeCount()
        {
            this.Time = string.Empty;
            this.Count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCount"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="count">The count.</param>
        public TimeCount(string time, int count)
        {
            this.Time = time;
            this.Count = count;
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }

    /// <summary>
    /// Class Sentiment.
    /// </summary>
    public class Sentiment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sentiment"/> class.
        /// </summary>
        public Sentiment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sentiment"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="positiveCount">The positive count.</param>
        /// <param name="negativeCount">The negative count.</param>
        public Sentiment(string time, int positiveCount, int negativeCount)
        {
            this.Time = time;
            this.PositiveCount = positiveCount;
            this.NegativeCount = negativeCount;
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the positive count.
        /// </summary>
        /// <value>The positive count.</value>
        public int PositiveCount { get; set; }

        /// <summary>
        /// Gets or sets the negative count.
        /// </summary>
        /// <value>The negative count.</value>
        public int NegativeCount { get; set; }
    }

    /// <summary>
    /// Class DayHourCount.
    /// </summary>
    public class DayHourCount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DayHourCount"/> class.
        /// </summary>
        public DayHourCount()
        {
            this.DaySequence = 0;
            this.Hour = 0;
            this.Count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayHourCount"/> class.
        /// </summary>
        /// <param name="daySequence">The day sequence.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="count">The count.</param>
        /// <param name="datetime">The datetime.</param>
        public DayHourCount(int daySequence, int hour, int count, DateTime datetime)
        {
            this.DaySequence = daySequence;
            this.Hour = hour;
            this.Count = count;
            this.date = datetime;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime date { get; set; }

        //when used when query DB, set to 0:0:0,  will add to hh:00:00 when sent to frontend

        /// <summary>
        /// Gets or sets the day sequence.
        /// </summary>
        /// <value>The day sequence.</value>
        public int DaySequence { get; set; } //0-6

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public int Hour { get; set; } //0-23

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }

    /// <summary>
    /// Class NameCount.
    /// </summary>
    public class NameCount
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }

    /// <summary>
    /// Class AgeGenderCount.
    /// </summary>
    public class AgeGenderCount
    {
        /// <summary>
        /// Gets or sets the age group.
        /// </summary>
        /// <value>The age group.</value>
        public string AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }

    /// <summary>
    /// Class WeeklyReportModel.
    /// </summary>
    public class WeeklyReportModel
    {
        /// <summary>
        /// Gets or sets the report count trend.
        /// </summary>
        /// <value>The report count trend.</value>
        public IList<TimeCount> ReportCountTrend { get; set; }

        /// <summary>
        /// Gets or sets the visit count trend.
        /// </summary>
        /// <value>The visit count trend.</value>
        public IEnumerable<TimeCount> VisitCountTrend { get; set; }

        /// <summary>
        /// Gets or sets the visit sentiment trend.
        /// </summary>
        /// <value>The visit sentiment trend.</value>
        public IEnumerable<Sentiment> VisitSentimentTrend { get; set; }

        /// <summary>
        /// Gets or sets the top news source.
        /// </summary>
        /// <value>The top news source.</value>
        public IEnumerable<NameCount> TopNewsSource { get; set; }

        /// <summary>
        /// Gets or sets the age gender count.
        /// </summary>
        /// <value>The age gender count.</value>
        public IEnumerable<AgeGenderCount> AgeGenderCount { get; set; }

        //public IEnumerable<NameCount> GenderCount { get; set; }

        //public IEnumerable<NameCount> AgeCount { get; set; }
    }
}
// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ModelConverter.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System;
    using System.Collections.Generic;

    using DataAccessLayer.DataModels;

    /// <summary>
    /// Class ModelConverter.
    /// </summary>
    public class ModelConverter
    {
        /// <summary>
        /// The defaultthumbnailurl
        /// </summary>
        private const string DEFAULTTHUMBNAILURL = @"/Resources/img/sample.png";
        private const string DEFAULTNEWSURL = @"/Resources/img/sample_new.jpg";

        /// <summary>
        /// To the weibo brief.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>WeiboBrief.</returns>
        public static WeiboBrief ToWeiboBrief(WeiboFilterPredictResults input)
        {
            var result = new WeiboBrief();
            result.Text = input.GetText(input.Text, input.RetweetedText);
            ;
            result.Title = input.Topic;
            result.WeiboId = input.WeiboId;
            result.WeiboCreatedTime = input.WeiboCreatedTime;
            result.Thumbnail = string.IsNullOrEmpty(input.ThumbnailPic) ? DEFAULTTHUMBNAILURL : input.ThumbnailPic;
            return result;
        }

        /// <summary>
        /// To the weibo detail.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>WeiboDetail.</returns>
        public static WeiboDetail ToWeiboDetail(WeiboFilterPredictResults input)
        {
            var result = new WeiboDetail();
            result.WeiboContent = input.GetText(input.Text, input.RetweetedText);
            result.WeiboCreatedTime = input.WeiboCreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
            result.WeiboId = input.WeiboId;
            result.WeiboPublishingSource = input.Source;
            result.WeiboTitle = input.GetTopic(input.Text, input.RetweetedText);
            result.UserName = input.UserName;
            result.Thumbnail = string.IsNullOrEmpty(input.ThumbnailPic) ? DEFAULTTHUMBNAILURL : input.ThumbnailPic;
            return result;
        }

        /// <summary>
        /// To the sentiment scan report.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>SentimentScanReport.</returns>
        public static SentimentScanReport ToSentimentScanReport(SentimentScanResult input)
        {
            SentimentScanReport report = null;
            var total = input.Negative + input.Positive;
            if (total > 0)
            {
                report = new SentimentScanReport();
                report.NegativePerct = GetRate(input.Negative, total);
                report.PositivePerct = GetRate(input.Positive, total);
                report.Score = input.Score;
            }

            return report;
        }

        /// <summary>
        /// To the news brief.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>NewsBrief.</returns>
        public static NewsBrief ToNewsBrief(NewsStream input)
        {
            var result = new NewsBrief();
            result.ClusterId0 = input.ClusterId0;
            result.Description = input.NewsArticleDescription;
            result.Id = input.Id;
            result.Source = input.NewsSource;
            result.ThumbnailUrl = string.IsNullOrEmpty(input.GoodDominantImageURL)
                                      ? DEFAULTNEWSURL
                                      : input.GoodDominantImageURL;
            result.Title = input.Title;
            result.Url = input.Url;
            result.CreatedTime = input.BuildTime;
            return result;
        }

        /// <summary>
        /// To the news brief list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public static List<NewsBrief> ToNewsBriefList(List<NewsStream> input)
        {
            List<NewsBrief> result = null;
            if (input != null && input.Count > 0)
            {
                result = new List<NewsBrief>();
                foreach (var item in input)
                {
                    result.Add(ToNewsBrief(item));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the rate.
        /// </summary>
        /// <param name="numberator">The numberator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns>System.Double.</returns>
        private static double GetRate(int numberator, int denominator)
        {
            return Math.Round(numberator / (double)denominator, 2);
        }
    }
}
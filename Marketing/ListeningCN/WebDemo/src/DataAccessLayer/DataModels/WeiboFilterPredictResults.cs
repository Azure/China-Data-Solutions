// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboFilterPredictResults.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using DataAccessLayer.Helper;

    /// <summary>
    /// Class WeiboFilterPredictResults.
    /// </summary>
    [TableName(TableName = "WeiboFilterPredictResults")]
    public class WeiboFilterPredictResults
    {
        /// <summary>
        /// The topic
        /// </summary>
        protected string topic;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [DBNotMap]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the source predict identifier.
        /// </summary>
        /// <value>The source predict identifier.</value>
        public long SourcePredictId { get; set; }

        /// <summary>
        /// Gets or sets the message window identifier.
        /// </summary>
        /// <value>The message window identifier.</value>
        public long MessageWindowId { get; set; }

        /// <summary>
        /// Gets or sets the key words.
        /// </summary>
        /// <value>The key words.</value>
        public string KeyWords { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the weibo text.
        /// </summary>
        /// <value>The weibo text.</value>
        public string WeiboText { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>The topic.</value>
        [DBNotMap]
        public string Topic
        {
            get
            {
                if (string.IsNullOrEmpty(this.topic))
                {
                    this.topic = this.GetTopic(this.Text, this.RetweetedText);
                }

                return this.topic;
            }
            set
            {
                this.topic = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [Column("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the predicting rank.
        /// </summary>
        /// <value>The predicting rank.</value>
        public double PredictingRank { get; set; }

        /// <summary>
        /// Gets or sets the weibo created time.
        /// </summary>
        /// <value>The weibo created time.</value>
        public DateTime WeiboCreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the weibo identifier.
        /// </summary>
        /// <value>The weibo identifier.</value>
        public long WeiboId { get; set; }

        /// <summary>
        /// Gets or sets the retweeted message identifier.
        /// </summary>
        /// <value>The retweeted message identifier.</value>
        [Column("retweeted_message_id")]
        public long RetweetedMessageId { get; set; }

        /// <summary>
        /// Gets or sets the retweeted text.
        /// </summary>
        /// <value>The retweeted text.</value>
        [Column("retweeted_text")]
        public string RetweetedText { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail pic.
        /// </summary>
        /// <value>The thumbnail pic.</value>
        [Column("thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        /// <summary>
        /// Gets or sets the original pic.
        /// </summary>
        /// <value>The original pic.</value>
        [Column("original_pic")]
        public string OriginalPic { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <param name="retweetedText">The retweeted text.</param>
        /// <returns>System.String.</returns>
        public string GetText(string sourceText, string retweetedText)
        {
            return string.IsNullOrEmpty(retweetedText) ? sourceText : retweetedText;
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <param name="retweetedText">The retweeted text.</param>
        /// <returns>System.String.</returns>
        public string GetTopic(string sourceText, string retweetedText)
        {
            var text = this.GetText(sourceText, retweetedText);
            var startSign = 0;
            var endSign = 0;
            startSign = text.IndexOf("【", StringComparison.Ordinal);
            endSign = text.IndexOf("】", StringComparison.Ordinal);
            if (startSign >= 0 && endSign > startSign) return text.Substring(startSign + 1, endSign - startSign - 1);
            return string.Empty;
        }
    }
}
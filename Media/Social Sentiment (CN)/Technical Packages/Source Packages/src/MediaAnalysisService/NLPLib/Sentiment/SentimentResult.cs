namespace NLPLib.Sentiment
{
    public class SentimentResult
    {
        /// <summary>
        ///     Gets or sets the input string.
        /// </summary>
        /// <value>The input string.</value>
        public string InputString { get; set; }

        /// <summary>
        ///     Gets or sets the type of the sentiment.
        /// </summary>
        /// <value>The type of the sentiment.</value>
        public string SentimentType { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public decimal Score { get; set; }
    }
}
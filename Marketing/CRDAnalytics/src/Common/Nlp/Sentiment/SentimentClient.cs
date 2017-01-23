// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Nlp.Sentiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using Configurations;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the sentiment client class.
    /// </summary>
    public static class SentimentClient
    {
        #region Fields

        /// <summary>
        /// The JSON media type
        /// </summary>
        private static readonly MediaTypeWithQualityHeaderValue JsonMediaType =
            new MediaTypeWithQualityHeaderValue(@"application/json")
            {
                CharSet = @"utf-8"
            };

        /// <summary>
        /// The form-url-encoded media type
        /// </summary>
        private static readonly MediaTypeWithQualityHeaderValue FormUrlEncodedMediaType =
            new MediaTypeWithQualityHeaderValue(@"application/x-www-form-urlencoded")
            {
                CharSet = @"utf-8"
            };

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="SentimentClient"/> class.
        /// </summary>
        static SentimentClient()
        {
            // Temporary work around, bypass the certification validation.
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sentiment service endpoint.
        /// </summary>
        /// <value>
        /// The sentiment service endpoint.
        /// </value>
        private static string SentimentServiceEndpoint =>
            ConfigurationService.GetSetting(ConfigurationSettingNames.SentimentServiceEndpoint);

        /// <summary>
        /// Gets the sentiment service single analyze endpoint.
        /// </summary>
        /// <value>
        /// The sentiment service single analyze endpoint.
        /// </value>
        private static string SentimentServiceSingleAnalyzeEndpoint =>
            SentimentServiceEndpoint + @"api/sentiment/analyze";

        /// <summary>
        /// Gets the sentiment service batch analyze endpoint.
        /// </summary>
        /// <value>
        /// The sentiment service batch analyze endpoint.
        /// </value>
        private static string SentimentServiceBatchAnalyzeEndpoint =>
            SentimentServiceEndpoint + @"api/sentiment/batchanalyze";

        #endregion

        #region Methods

        /// <summary>
        /// Analyzes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The sentiment result.</returns>
        public static SentimentResult Analyze(string text) =>
            AnalyzeAsync(text).GetAwaiter().GetResult();

        /// <summary>
        /// Analyzes the asynchronous.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>
        /// The sentiment result.
        /// </returns>
        public static async Task<SentimentResult> AnalyzeAsync(string text, int retryCount = 0)
        {
            const int MaxRetryCount = 3;

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // The expected response format is JSON
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(JsonMediaType);

                    // The expected request format is Form URL Encoded
                    var content = new StringContent(@"=" + Uri.EscapeDataString(text));
                    content.Headers.ContentType = FormUrlEncodedMediaType;

                    var response = await client.PostAsync(SentimentServiceSingleAnalyzeEndpoint, content);

                    response.EnsureSuccessStatusCode();

                    var responseText = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<SentimentResult>(responseText);
                }
            }
            catch (Exception)
            {
                if (retryCount >= MaxRetryCount)
                {
                    throw;
                }

                Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2, retryCount)));
                return await AnalyzeAsync(text, ++retryCount);
            }
        }

        /// <summary>
        /// Batches the analyze asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="textDictionary">The text dictionary.</param>
        /// <returns>The sentiment results.</returns>
        public static async Task<IDictionary<TKey, SentimentResult>> BatchAnalyzeAsync<TKey>(
            IDictionary<TKey, string> textDictionary)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                // The expected response format is JSON
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(JsonMediaType);

                // The expected request format is JSON
                var content = new StringContent(JsonConvert.SerializeObject(new { Body = textDictionary.ToList() }));
                content.Headers.ContentType = JsonMediaType;

                var response = await client.PostAsync(SentimentServiceBatchAnalyzeEndpoint, content);

                response.EnsureSuccessStatusCode();

                var responseText = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<TKey, SentimentResult>>>(responseText);

                return result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        #endregion
    }
}

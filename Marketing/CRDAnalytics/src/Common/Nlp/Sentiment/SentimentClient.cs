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
    using Extensions;
    using log4net;
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

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SentimentClient));

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
        /// <returns>
        /// The sentiment result.
        /// </returns>
        public static async Task<SentimentResult> AnalyzeAsync(string text) =>
            await RunAsyncActionWithRetry(
                text,
                async delegate(string input)
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);

                        // The expected response format is JSON
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(JsonMediaType);

                        // The expected request format is Form URL Encoded
                        var content = new StringContent(@"=" + Uri.EscapeDataString(input));
                        content.Headers.ContentType = FormUrlEncodedMediaType;

                        var response = await client.PostAsync(SentimentServiceSingleAnalyzeEndpoint, content);

                        response.EnsureSuccessStatusCode();

                        var responseText = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<SentimentResult>(responseText);
                    }
                },
                ex =>
                    $"Invoke sentiment service {SentimentServiceSingleAnalyzeEndpoint} failed, detail: {ex.GetDetailMessage()}",
                (ex, retryCount) =>
                    $"Invoke sentiment service {SentimentServiceSingleAnalyzeEndpoint} failed {retryCount} time(s), detail: {ex.GetDetailMessage()}");

        /// <summary>
        /// Batches the analyze asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="textDictionary">The text dictionary.</param>
        /// <returns>The sentiment results.</returns>
        public static async Task<IDictionary<TKey, SentimentResult>> BatchAnalyzeAsync<TKey>(
            IDictionary<TKey, string> textDictionary) =>
            await RunAsyncActionWithRetry(
                textDictionary,
                async delegate(IDictionary<TKey, string> input)
                {
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(30);

                        // The expected response format is JSON
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(JsonMediaType);

                        // The expected request format is JSON
                        var content = new StringContent(JsonConvert.SerializeObject(new { Body = input.ToList() }));
                        content.Headers.ContentType = JsonMediaType;

                        var response = await client.PostAsync(SentimentServiceBatchAnalyzeEndpoint, content);

                        response.EnsureSuccessStatusCode();

                        var responseText = await response.Content.ReadAsStringAsync();

                        var result = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<TKey, SentimentResult>>>(responseText);

                        return result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    }
                },
                ex =>
                    $"Invoke sentiment service {SentimentServiceBatchAnalyzeEndpoint} failed, detail: {ex.GetDetailMessage()}",
                (ex, retryCount) =>
                    $"Invoke sentiment service {SentimentServiceBatchAnalyzeEndpoint} failed {retryCount} time(s), detail: {ex.GetDetailMessage()}");

        /// <summary>
        /// Runs the asynchronous action with retry.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="action">The action.</param>
        /// <param name="errorMessageGetter">The error message getter.</param>
        /// <param name="warnMessageGetter">The warn message getter.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <returns>The action result.</returns>
        private static async Task<TOutput> RunAsyncActionWithRetry<TInput, TOutput>(
            TInput input,
            Func<TInput, Task<TOutput>> action,
            Func<Exception, string> errorMessageGetter,
            Func<Exception, int, string> warnMessageGetter,
            int retryCount = 0)
        {
            const int MaxRetryCount = 3;

            try
            {
                return await action(input);
            }
            catch (Exception ex)
            {
                if (retryCount >= MaxRetryCount)
                {
                    Logger.Error(errorMessageGetter(ex), ex);

                    throw;
                }

                Logger.Warn(warnMessageGetter(ex, retryCount), ex);

                Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2, retryCount)));
                return await RunAsyncActionWithRetry(input, action, errorMessageGetter, warnMessageGetter, ++retryCount);
            }
        }

        #endregion
    }
}

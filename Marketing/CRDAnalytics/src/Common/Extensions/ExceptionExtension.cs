// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Extensions
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Defines the exception extension class.
    /// </summary>
    public static class ExceptionExtension
    {
        #region Fields

        #region Fields => String Constants for General Exception

        /// <summary>
        /// The inner exception separation line.
        /// </summary>
        private const string InnerExceptionSeparationLine = @"==========Inner Exceptions==========";

        /// <summary>
        /// The exception type format.
        /// </summary>
        private const string ExceptionTypeFormat = @"Exception Type: {0}";

        /// <summary>
        /// The exception message format.
        /// </summary>
        private const string ExceptionMessageFormat = @"Exception Message: {0}";

        /// <summary>
        /// The exception stack trace format.
        /// </summary>
        private const string ExceptionStacktraceFormat = @"Exception StackTrace: {0}";

        #endregion

        #region Fields => String Constants for Web Exception

        /// <summary>
        /// The web exception header line.
        /// </summary>
        private const string WebExceptionHeaderLine = @"### Web Exception Detail ###";

        /// <summary>
        /// The status format.
        /// </summary>
        private const string StatusFormat = @"Status: {0}";

        /// <summary>
        /// The response URI format.
        /// </summary>
        private const string ResponseUriFormat = @"Response Uri: {0}";

        /// <summary>
        /// The response headers format.
        /// </summary>
        private const string ResponseHeadersFormat = @"Response Headers: {0}";

        /// <summary>
        /// The response body format.
        /// </summary>
        private const string ResponseBodyFormat = @"Response Body: {0}";

        #endregion

        #region Fields => String Constants for SQL Exception

        /// <summary>
        /// The SQL exception header line.
        /// </summary>
        private const string SqlExceptionHeaderLine = @"### SQL Exception Detail ###";

        /// <summary>
        /// The client connection identifier format.
        /// </summary>
        private const string ClientConnectionIdFormat = @"ClientConnectionId: {0}";

        /// <summary>
        /// The server format.
        /// </summary>
        private const string ServerFormat = @"Server: {0}";

        /// <summary>
        /// The class format.
        /// </summary>
        private const string ClassFormat = @"Class: {0}";

        /// <summary>
        /// The state format.
        /// </summary>
        private const string StateFormat = @"State: {0}";

        /// <summary>
        /// The SQL error number format.
        /// </summary>
        private const string SqlErrorNumberFormat = @"SQL Error: #{0:D2}";

        /// <summary>
        /// The error message format.
        /// </summary>
        private const string ErrorMessageFormat = @"Error Message: {0}";

        /// <summary>
        /// The error number format.
        /// </summary>
        private const string ErrorNumberFormat = @"Error Number: {0}";

        /// <summary>
        /// The line number format.
        /// </summary>
        private const string LineNumberFormat = @"LineNumber: {0}";

        /// <summary>
        /// The source format.
        /// </summary>
        private const string SourceFormat = @"Source: {0}";

        /// <summary>
        /// The procedure format.
        /// </summary>
        private const string ProcedureFormat = @"Procedure: {0}";

        #endregion

        #endregion

        #region Methods

        #region Methods => Public Static Methods

        /// <summary>
        /// Get exception detail message.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDetailMessage(this Exception exception)
        {
            var stringBuilder = new StringBuilder(256);

            PrintException(stringBuilder, exception);

            var currentException = exception.InnerException;
            if (currentException != null)
            {
                stringBuilder.AppendLine(InnerExceptionSeparationLine);
            }

            while (currentException != null)
            {
                stringBuilder.AppendLine();
                PrintException(stringBuilder, currentException);

                currentException = currentException.InnerException;
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region Methods => Private Static Methods

        /// <summary>
        /// Prints the web exception action.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="webException">The web exception.</param>
        private static void PrintWebException(StringBuilder stringBuilder, WebException webException)
        {
            stringBuilder.AppendLine(WebExceptionHeaderLine).AppendFormatLine(StatusFormat, webException.Status);

            var webResponse = webException.Response;

            if (webResponse == null)
            {
                return;
            }

            stringBuilder.AppendFormatLine(ResponseUriFormat, webResponse.ResponseUri);

            var headers =
                webResponse.Headers.AllKeys.ToDictionary(key => key, key => webResponse.Headers[key]).ToJsonIndented();
            stringBuilder.AppendFormatLine(ResponseHeadersFormat, headers);

            var responseStream = webResponse.GetResponseStream();
            if (responseStream != null)
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    stringBuilder.AppendFormatLine(ResponseBodyFormat, streamReader.ReadToEnd());
                }
            }

            webResponse.Close();
        }

        /// <summary>
        /// Prints the SQL exception action.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="sqlException">The SQL exception.</param>
        private static void PrintSqlException(StringBuilder stringBuilder, SqlException sqlException)
        {
            stringBuilder.AppendLine(SqlExceptionHeaderLine)
                .AppendFormatLine(ClientConnectionIdFormat, sqlException.ClientConnectionId)
                .AppendFormatLine(ServerFormat, sqlException.Server)
                .AppendFormatLine(ClassFormat, sqlException.Class)
                .AppendFormatLine(StateFormat, sqlException.State);

            for (var i = 0; i < sqlException.Errors.Count; i++)
            {
                var sqlError = sqlException.Errors[i];

                stringBuilder.AppendFormatLine(SqlErrorNumberFormat, i)
                    .AppendFormatLine(ErrorMessageFormat, sqlError.Message)
                    .AppendFormatLine(ErrorNumberFormat, sqlError.Number)
                    .AppendFormatLine(LineNumberFormat, sqlError.LineNumber)
                    .AppendFormatLine(SourceFormat, sqlError.Source)
                    .AppendFormatLine(ProcedureFormat, sqlError.Procedure);
            }
        }

        /// <summary>
        /// Prints the exception action.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="exception">The exception.</param>
        private static void PrintException(StringBuilder stringBuilder, Exception exception)
        {
            stringBuilder.AppendFormatLine(ExceptionTypeFormat, exception.GetType().AssemblyQualifiedName)
                .AppendFormatLine(ExceptionMessageFormat, exception.Message)
                .AppendFormatLine(ExceptionStacktraceFormat, exception.StackTrace);

            var webException = exception as WebException;
            if (webException != null)
            {
                PrintWebException(stringBuilder, webException);
            }

            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                PrintSqlException(stringBuilder, sqlException);
            }
        }

        /// <summary>
        /// Appends the format line.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        private static StringBuilder AppendFormatLine(
            this StringBuilder stringBuilder,
            string format,
            params object[] args)
        {
            return stringBuilder.AppendFormat(format, args).AppendLine();
        }

        #endregion

        #endregion
    }
}

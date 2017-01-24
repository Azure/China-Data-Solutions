// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.AppServiceHost
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    using Common.Extensions;
    using Common.Pipelines;
    using log4net;
    using log4net.Appender;
    using log4net.Repository.Hierarchy;

    /// <summary>
    /// Defines the global HTTP application.
    /// </summary>
    /// <seealso cref="HttpApplication" />
    public class Global : HttpApplication
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Global));

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            RefreshLogSettings();

            Logger.Info(@"Application starting...");

            try
            {
                PipelineManager.StartCustomerReviewDataPipeline();
            }
            catch (Exception exception)
            {
                Logger.Error(
                    $"Customer review data pipeline started failed, exception detail: {exception.GetDetailMessage()}",
                    exception);
            }

            Logger.Info(@"Application started...");
        }

        /// <summary>
        /// Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Response.Clear();

            var errorMessage = exception?.Message ?? "Internal error occurred.";

            Logger.Error(exception == null ? errorMessage : exception.GetDetailMessage(), exception);
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            Logger.Info(@"Application stopped...");
        }

        /// <summary>
        /// Refreshes the log settings.
        /// </summary>
        private static void RefreshLogSettings()
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy == null || !hierarchy.Configured)
            {
                return;
            }

            var defaultConnection =
                ConfigurationManager.ConnectionStrings[@"DatabaseConnectionString"].ConnectionString;
            foreach (var adoNetAppender in hierarchy.GetAppenders().OfType<AdoNetAppender>())
            {
                adoNetAppender.ConnectionString = defaultConnection;
                adoNetAppender.ActivateOptions();
            }
        }

        #endregion
    }
}
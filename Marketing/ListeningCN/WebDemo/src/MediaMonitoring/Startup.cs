// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="Startup.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using MediaMonitoring;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MediaMonitoring
{
    using DataAccessLayer;
    using DataAccessLayer.Helper;

    using Hangfire;
    using Hangfire.SqlServer;

    using MediaMonitoring.Utility;

    using Owin;

    /// <summary>
    /// Class Startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            var storage = GlobalConfiguration.Configuration.UseSqlServerStorage(SysConfig.DefaultConnStr);
            var options = new BackgroundJobServerOptions { Queues = new[] { "critical", "default" }, WorkerCount = 2 };
            app.UseHangfireDashboard();
            app.UseHangfireServer(options);
            ProfileHelper.InitClientUser();
            JobStorage.Current = storage.Entry;
        }
    }
}
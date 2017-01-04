// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 09-02-2016
// ***********************************************************************
// <copyright file="TaskProcessor.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace MediaMonitoring.Utility
{
    using System;

    using DataAccessLayer;
    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels;
    using DataAccessLayer.Helper;
    using DataAccessLayer.Managers;

    using Hangfire;
    using Hangfire.SqlServer;

    using Medallion.Threading.Sql;

    using Quartz;
    using Quartz.Impl;

    /// <summary>
    /// Class TaskProcessor.
    /// </summary>
    public class TaskProcessor
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            ScheduleJob<DataSyncJob>(scheduler, SysConfig.DataSyncInterval);
            ScheduleJob<WordCloudJob>(scheduler, SysConfig.WordCloudInterval);
            ScheduleJob<AnalysisJob>(scheduler, SysConfig.ReportsInterval);

            if (SysConfig.SyncWeibo)
            {
              //  ScheduleJob<WeiboSyncJob>(scheduler, SysConfig.WeiboInterval);
            }
        }

        /// <summary>
        /// Schedules the job.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="intervalinSeconds">The intervalin seconds.</param>
        internal static void ScheduleJob<T>(IScheduler scheduler, int intervalinSeconds) where T : IJob
        {
            var syncjob = JobBuilder.Create<T>().Build();

            var synctrigger =
                TriggerBuilder.Create()
                    .StartAt(DateBuilder.FutureDate(new Random().Next(3, 10), IntervalUnit.Minute))
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(intervalinSeconds).RepeatForever())
                    .Build();

            scheduler.ScheduleJob(syncjob, synctrigger);
        }

        /// <summary>
        /// Initializes the data fast.
        /// </summary>
        /// <param name="user">The user.</param>
        [Queue("critical")]
        public static void InitDataFast(ClientUser user)
        {
            var config = new SysConfig();
            var syncManager = new DataSyncManager();
            syncManager.ClearUserTables(user.GetProfile().Postfix);

           InitData(user);
           System.Threading.Thread.Sleep(120000);
            var date = DateTime.UtcNow;
            if (SysConfig.UseStaticDate)
            {
                date = new CompetitorDataGenerator(config, user).GetMaxDate();
            }
            
            BuildReports(config, user.GetProfile(), date);
            GenerateWordCloud(user.GetProfile());
        }

        /// <summary>
        /// Initializes the data.
        /// </summary>
        /// <param name="clientUser">The client user.</param>
        public static void InitData(ClientUser clientUser)
        {
            var distributedLock = new SqlDistributedLock("InitData_" + clientUser.Name, SysConfig.DefaultConnStr);

            using (distributedLock.Acquire())
            {
                try
                {
                    clientUser = ProfileHelper.GetClientUser(clientUser.Name);
                    var syncManager = new DataSyncManager();
                    syncManager.LoadUserData(
                        clientUser.GetProfile().Postfix,
                        clientUser.UserFilter,
                        clientUser.CompetitorFilter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Builds the reports.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="date">The date.</param>
        public static void BuildReports(SysConfig config, ClientUserProfile profile, DateTime date)
        {
            var distributedLock = new SqlDistributedLock("BuildReport_" + profile.UserName, SysConfig.DefaultConnStr);
            using (distributedLock.Acquire())
            {
                var dataGenerator = new CompetitorDataGenerator(config, new ClientUser(profile));
                var weeklyGenerator = new WeeklyReportDataGenerator(config, new ClientUser(profile));

                dataGenerator.GenerateAndSaveNewsList(date);
                dataGenerator.GenerateAndSaveTopSentiNews(date);
                dataGenerator.GenerateAndSaveEvent(date);
                dataGenerator.GenerateAndSaveMedia(date);
                dataGenerator.GenerateAndSaveSentiment(date);
                dataGenerator.GenerateAndSaveMap(date);
                dataGenerator.GenerateAndSaveAge(date);

                weeklyGenerator.GenerateAndSaveEvent(date);
                weeklyGenerator.GenerateAndSaveNewsList(date);
                weeklyGenerator.GenerateAndSaveWeeklyReport(date);
            }
        }

        /// <summary>
        /// Generates the word cloud.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public static void GenerateWordCloud(ClientUserProfile profile)
        {
            var distributedLock = new SqlDistributedLock("WordClouds_" + profile.UserName, SysConfig.DefaultConnStr);
            using (distributedLock.Acquire())
            {
                var clientUser = new ClientUser(profile);
                var wcManager = new WordCloudManager();
                wcManager.GenerateIndexedResult(clientUser, new ClientUser(profile).UserFilter);
            }
        }

        /// <summary>
        /// Class DataSyncJob.
        /// </summary>
        /// <seealso cref="Quartz.IJob" />
        private class DataSyncJob : IJob
        {
            /// <summary>
            /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
            /// fires that is associated with the <see cref="T:Quartz.IJob" />.
            /// </summary>
            /// <param name="context">The execution context.</param>
            /// <remarks>The implementation may wish to set a  result object on the
            /// JobExecutionContext before this method exits.  The result itself
            /// is meaningless to Quartz, but may be informative to
            /// <see cref="T:Quartz.IJobListener" />s or
            /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
            /// execution.</remarks>
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    var profiles = ProfileHelper.GetAllProfiles();
                    foreach (var profile in profiles)
                    {
                        BackgroundJob.Enqueue(() => InitData(new ClientUser(profile)));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Class WeiboSyncJob.
        /// </summary>
        /// <seealso cref="Quartz.IJob" />
        private class WeiboSyncJob : IJob
        {
            /// <summary>
            /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
            /// fires that is associated with the <see cref="T:Quartz.IJob" />.
            /// </summary>
            /// <param name="context">The execution context.</param>
            /// <remarks>The implementation may wish to set a  result object on the
            /// JobExecutionContext before this method exits.  The result itself
            /// is meaningless to Quartz, but may be informative to
            /// <see cref="T:Quartz.IJobListener" />s or
            /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
            /// execution.</remarks>
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    var profiles = ProfileHelper.GetAllProfiles();
                    var weiboManager = new WeiboSyncManager();
                    {
                        weiboManager.SaveFilterResults(profiles);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Class WordCloudJob.
        /// </summary>
        /// <seealso cref="Quartz.IJob" />
        private class WordCloudJob : IJob
        {
            /// <summary>
            /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
            /// fires that is associated with the <see cref="T:Quartz.IJob" />.
            /// </summary>
            /// <param name="context">The execution context.</param>
            /// <remarks>The implementation may wish to set a  result object on the
            /// JobExecutionContext before this method exits.  The result itself
            /// is meaningless to Quartz, but may be informative to
            /// <see cref="T:Quartz.IJobListener" />s or
            /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
            /// execution.</remarks>
            public void Execute(IJobExecutionContext context)
            {
                var profiles = ProfileHelper.GetAllProfiles();
                foreach (var profile in profiles)
                {
                    try
                    {
                        BackgroundJob.Enqueue(() => GenerateWordCloud(profile));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Class AnalysisJob.
        /// </summary>
        /// <seealso cref="Quartz.IJob" />
        private class AnalysisJob : IJob
        {
            /// <summary>
            /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
            /// fires that is associated with the <see cref="T:Quartz.IJob" />.
            /// </summary>
            /// <param name="context">The execution context.</param>
            /// <remarks>The implementation may wish to set a  result object on the
            /// JobExecutionContext before this method exits.  The result itself
            /// is meaningless to Quartz, but may be informative to
            /// <see cref="T:Quartz.IJobListener" />s or
            /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
            /// execution.</remarks>
            public void Execute(IJobExecutionContext context)
            {
                var date = DateTime.UtcNow.AddHours(2);
                var profiles = ProfileHelper.GetAllProfiles();
                var config = new SysConfig();

                foreach (var profile in profiles)
                {
                    try
                    {
                        if (SysConfig.UseStaticDate)
                        {
                            date = new CompetitorDataGenerator(config, new ClientUser(profile)).GetMaxDate();
                        }

                        BackgroundJob.Enqueue(() => BuildReports(config, profile, date));
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
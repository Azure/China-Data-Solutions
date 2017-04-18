using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace DataPrepare
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessDataGenerateAndClean([TimerTrigger(typeof(CustomSchedule), RunOnStartup = true, UseMonitor = true)] TimerInfo tinmerInfo, TextWriter log)
        {
            try
            {
                //Wait for configuration ready
                System.Threading.Thread.Sleep(1000 * 120);

                DataPrepareWorker prepareWorker = new DataPrepareWorker();
                prepareWorker.Run();

                WeiboCleanWorker weibocleaner = new WeiboCleanWorker();
                weibocleaner.Run();

                WeiboAnalysisWorker anlyzeWorker = new WeiboAnalysisWorker();
                anlyzeWorker.Run().Wait();
            }
            catch (Exception e)
            {
                Logger.Log(e);
                log.WriteLine(e.StackTrace);
            }
        }


        public class CustomSchedule : TimerSchedule
        {
            public override DateTime GetNextOccurrence(DateTime now)
            {
                return new DateTime(2099, 5, 22, 9, 45, 00);
            }
        }
    }
}

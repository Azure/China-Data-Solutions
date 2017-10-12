using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaAnalysis;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace DataCollectingJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessDataGeneration([TimerTrigger( typeof(CustomSchedule) , RunOnStartup =true, UseMonitor =true)] TimerInfo tinmerInfo, TextWriter log)
        {
            try
            {
                DataSimulator simulator = new DataSimulator();
                simulator.Run();
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

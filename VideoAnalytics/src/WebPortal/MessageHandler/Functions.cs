using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace MessageHandler
{
    public class Functions
    {
        public static void RunningEventProcessing([TimerTrigger(typeof(OneTimeSchedule), RunOnStartup = true)] TimerInfo timerInfo)
        {
            Console.WriteLine("CustomTimerJobFunctionDaily ran at : " + DateTime.UtcNow);
            EventHubMessageProcessor processor = new EventHubMessageProcessor();
            processor.StartMessageProcess();
        }


        public class OneTimeSchedule : TimerSchedule
        {
            public override DateTime GetNextOccurrence(DateTime now)
            {
                return new DateTime(2199, 12, 31);
            }
        }
    }
}

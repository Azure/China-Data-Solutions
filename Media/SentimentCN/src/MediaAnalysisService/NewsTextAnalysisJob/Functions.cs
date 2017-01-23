using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace NewsTextAnalysisJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessNewsTextAnalysis([TimerTrigger("00:00:05", RunOnStartup = true, UseMonitor = true)] TimerInfo tinmerInfo, TextWriter log)
        {
            try
            {
                NewsAnalysisWorker worker = new NewsAnalysisWorker();
                worker.Run();
            }
            catch (Exception e)
            {
                log.WriteLine(e.Message);
                log.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

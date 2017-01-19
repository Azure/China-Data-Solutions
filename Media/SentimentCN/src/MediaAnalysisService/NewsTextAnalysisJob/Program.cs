using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaAnalysis;
using Microsoft.Azure.WebJobs;

namespace NewsTextAnalysisJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            try
            {
                JobHostConfiguration config = new JobHostConfiguration();
                config.UseTimers();
                var host = new JobHost(config);
                // The following code ensures that the WebJob will be running continuously
                host.RunAndBlock();
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
        }
    }
}

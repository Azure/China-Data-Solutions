using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.WindowsAzure.Storage;

namespace DataCollectingJob
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
                CloudStorageAccount ac;
                var result = CloudStorageAccount.TryParse(config.DashboardConnectionString, out ac);
                // The following code ensures that the WebJob will be running continuously
                host.RunAndBlock();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

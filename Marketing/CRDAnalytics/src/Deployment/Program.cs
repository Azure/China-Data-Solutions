namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Deployment
{
    using System;

    using Common.Pipelines;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                PipelineManager.StartCustomerReviewDataPipeline();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception Type: {exception.GetType().AssemblyQualifiedName}");
                Console.WriteLine($"Exception Message: {exception.Message}");
                Console.WriteLine($"Exception StackTrace: {exception.StackTrace}");

                if (exception.InnerException == null)
                {
                    return;
                }

                var currentException = exception.InnerException;

                Console.WriteLine(@"Inner Exception:");
                Console.WriteLine(new string('=', 80));

                while (currentException != null)
                {
                    Console.WriteLine($"Exception Type: {currentException.GetType().AssemblyQualifiedName}");
                    Console.WriteLine($"Exception Message: {currentException.Message}");
                    Console.WriteLine($"Exception StackTrace: {currentException.StackTrace}");
                    Console.WriteLine(new string('-', 80));

                    currentException = currentException.InnerException;
                }
            }

            Console.WriteLine(@"Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}

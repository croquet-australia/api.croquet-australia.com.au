using CroquetAustralia.Domain.Settings;
using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor
{
    public class Program
    {
        public static void Main()
        {
            // JobHost will search for and run methods that use the QueueTrigger attribute on its first parameter.
            // By "my" convention all web jobs are in the WebJobs namespace
            using (var host = new JobHost(GetJobHostConfiguration()))
            {
                host.RunAndBlock();
            }
        }

        private static JobHostConfiguration GetJobHostConfiguration()
        {
            var connectionStrings = new ConnectionStringSettings();
            var config = new JobHostConfiguration(connectionStrings.AzureStorage);

            return config;
        }
    }
}
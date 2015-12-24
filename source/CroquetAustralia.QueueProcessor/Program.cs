using System;
using Anotar.NLog;
using CroquetAustralia.Domain.Settings;
using Microsoft.Azure.WebJobs;

namespace CroquetAustralia.QueueProcessor
{
    public class Program
    {
        public static void Main()
        {
            LogTo.Info("Started QueueProcessor.");
            LogTo.Info($"Storage '{new ConnectionStringSettings().AzureStorage.Split(';')[1]}'.)");

            try
            {
                // JobHost will search for and run methods that use the QueueTrigger attribute on its first parameter.
                // By convention all web jobs are configured in Functions.cs
                using (var host = new JobHost(GetJobHostConfiguration()))
                {
                    host.RunAndBlock();
                }
            }
            catch (Exception exception)
            {
                LogTo.ErrorException("Bugger, exception was thrown.", exception);
                throw;
            }
            finally
            {
                LogTo.Info("Exiting QueueProcessor");
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
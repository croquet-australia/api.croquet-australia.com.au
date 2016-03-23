using System;
using System.Linq;
using Anotar.NLog;
using CroquetAustralia.Domain.Settings;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace CroquetAustralia.QueueProcessor
{
    public class Program
    {
        public static void Main(params string[] args)
        {
            LogTo.Info("Started QueueProcessor.");

            var handleExceptions = args != null && args.Contains("--handle-exceptions");

            try
            {
                var connectionStrings = new ConnectionStringSettings();
                var serviceProvider = new ServiceProvider();
                var jobActivator = new JobActivator(serviceProvider);
                var configuration = GetJobHostConfiguration(connectionStrings.AzureStorage, jobActivator);

                // JobHost will search for and run methods that use the QueueTrigger attribute on its first parameter.
                // By convention all web jobs are configured in Functions.cs
                using (var host = new JobHost(configuration))
                {
                    try
                    {
                        host.RunAndBlock();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"An exception was thrown by {host.GetType()}.", exception);
                    }
                }
            }
            catch (Exception exception)
            {
                LogTo.ErrorException(exception.Message, exception);

                if (handleExceptions)
                {
                    HandleException(exception);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                LogTo.Info($"Exiting {nameof(Main)}.");
            }
        }

        private static void HandleException(Exception exception)
        {
            var foregroundColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(exception.Message);
            Console.WriteLine();
            Console.WriteLine(exception);
            Console.WriteLine();

            Console.ForegroundColor = foregroundColor;
        }

        private static JobHostConfiguration GetJobHostConfiguration(string dashboardConnectionString, IJobActivator jobActivator)
        {
            var config = new JobHostConfiguration(dashboardConnectionString)
            {
                JobActivator = jobActivator
            };

            return config;
        }
    }
}
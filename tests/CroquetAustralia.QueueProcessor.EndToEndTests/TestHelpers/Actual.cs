using System;
using Anotar.NLog;

namespace CroquetAustralia.QueueProcessor.EndToEndTests.TestHelpers
{
    public class Actual
    {
        public Exception Exception { get; set; }
        public string MessageId { get; set; }

        public void Invoke(Action action)
        {
            try
            {
                Exception = null;
                action();
            }
            catch (Exception exception)
            {
                LogTo.ErrorException(exception.Message, exception);

                foreach (var key in exception.Data.Keys)
                {
                    LogTo.Error($"{key}: {exception.Data[key]}");
                }

                Exception = exception;
            }
        }
    }
}
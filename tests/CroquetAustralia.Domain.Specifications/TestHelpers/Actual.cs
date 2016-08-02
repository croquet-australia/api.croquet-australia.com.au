using System;
using Anotar.NLog;
using CroquetAustralia.Domain.ValueObjects;

namespace CroquetAustralia.Domain.Specifications.TestHelpers
{
    public class Actual
    {
        public Exception Exception { get; set; }
        public TournamentDateOfBirthRange DateOfBirthRange { get; set; }

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
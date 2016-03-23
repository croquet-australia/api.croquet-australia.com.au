using System;
using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.WebApi.Specifications.TestHelpers
{
    public class TestBase : CommonTestBase
    {
        public TestBase()
            : base(ServiceProvider.Instance)
        {
        }

        protected ICommand ValidCommand(string commandType)
        {
            if (commandType == "SubmitEntry")
            {
                return Valid<SubmitEntry>();
            }

            // todo: quick hack ready for future expansion
            throw new NotImplementedException($"ValidCommand({commandType}) is not implemented.");
        }
    }
}
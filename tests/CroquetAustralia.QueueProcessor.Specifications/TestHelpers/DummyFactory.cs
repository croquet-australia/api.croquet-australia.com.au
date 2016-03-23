using CroquetAustralia.Domain.Features.TournamentEntry.Commands;

namespace CroquetAustralia.QueueProcessor.Specifications.TestHelpers
{
    public class DummyFactory : Domain.UnitTests.TestHelpers.DummyFactory
    {
        public DummyFactory(ITournamentsRepository tournamentsRepository)
            : base(tournamentsRepository)
        {
        }
    }
}
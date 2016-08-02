using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.UnitTests.TestHelpers;
using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.Domain.Specifications.TestHelpers
{
    public class Given
    {
        public Given()
        {
            DummyTournament = ServiceProvider.Instance.Get<DummyTournament>();
        }

        public DummyTournament DummyTournament { get; set; }
        public Tournament Tournament { get; set; }
    }
}
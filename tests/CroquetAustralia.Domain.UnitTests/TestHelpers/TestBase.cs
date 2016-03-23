using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class TestBase : CommonTestBase
    {
        public TestBase()
            : base(ServiceProvider.Instance)
        {
        }
    }
}
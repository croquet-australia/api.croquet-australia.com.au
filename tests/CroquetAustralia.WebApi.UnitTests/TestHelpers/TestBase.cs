using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.WebApi.UnitTests.TestHelpers
{
    public class TestBase : CommonTestBase
    {
        public TestBase()
            : base(ServiceProvider.Instance)
        {
        }
    }
}
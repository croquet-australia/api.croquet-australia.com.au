using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers
{
    public class TestBase : CommonTestBase
    {
        public TestBase()
            : base(ServiceProvider.Instance)
        {
        }
    }
}
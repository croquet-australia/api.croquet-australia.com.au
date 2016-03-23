using CroquetAustralia.TestHelpers;

namespace CroquetAustralia.QueueProcessor.Specifications.TestHelpers
{
    public class TestBase : CommonTestBase
    {
        public TestBase()
            : base(ServiceProvider.Instance)
        {
        }
    }
}
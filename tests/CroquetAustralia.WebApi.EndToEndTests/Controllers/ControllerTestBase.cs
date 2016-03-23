using CroquetAustralia.WebApi.EndToEndTests.TestHelpers;

namespace CroquetAustralia.WebApi.EndToEndTests.Controllers
{
    public abstract class ControllerTestBase
    {
        protected WebApiClient WebApi;

        protected ControllerTestBase()
        {
            WebApi = new WebApiClient();
        }
    }
}
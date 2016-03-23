using System;
using System.Net;
using System.Net.Http;
using FluentAssertions.Execution;
using Newtonsoft.Json.Linq;
using OpenMagic.Exceptions;
using TechTalk.SpecFlow;

namespace CroquetAustralia.WebApi.Specifications.TestHelpers
{
    [Binding]
    public class CommonSteps : TestBase
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly WebApiClient _webApiClient;

        public CommonSteps(GivenData given, ActualData actual, WebApiClient webApiClient)
        {
            _given = given;
            _actual = actual;
            _webApiClient = webApiClient;
        }

        [Given(@"todo")]
        public void GivenTodo()
        {
            throw new ToDoException();
        }

        [Given(@"a valid '(.*)' command")]
        public void GivenAValidCommand(string commandType)
        {
            _given.Command = ValidCommand(commandType);
        }

        [When(@"the command is posted to '(.*)'")]
        public void WhenTheCommandIsPostedTo(string resource)
        {
            _actual.Response = _webApiClient.Post(resource, _given.Command);
        }

        [Then(@"the response status code should be '(.*)'")]
        public void ThenTheResponseStatusCodeShouldBe(string expectedStatusCodeName)
        {
            var expectedStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), expectedStatusCodeName);
            var actualStatusCode = _actual.Response.StatusCode;

            if (actualStatusCode == expectedStatusCode)
            {
                return;
            }

            var response = _actual.Response.Content.ReadAsAsync<JObject>().Result;
            var errorMessage = $"Expected StatusCode to be {expectedStatusCode}, but found {actualStatusCode}.\n\n{response}";

            throw new AssertionFailedException(errorMessage);
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CroquetAustralia.WebApi.EndToEndTests.Controllers
{
    public class TournamentEntryControllerTests : ControllerTestBase
    {
        public class AddEntryAsync : TournamentEntryControllerTests
        {
            private HttpResponseMessage PostInvalid_SubmitEntry_Command()
            {
                var command = InvalidSubmitEntryCommand();
                var response = WebApi.Post("/tournament-entry/add-entry", command);

                if (response.StatusCode != HttpStatusCode.InternalServerError)
                {
                    return response;
                }

                var content = response.Content.ReadAsAsync<JObject>().Result;
                var message = content["ExceptionMessage"];
                var stackTrace = content["StackTrace"];

                throw new Exception($"WebApi returned HttpStatusCode.InternalServerError.\n\n{message}\n\n{stackTrace}");
            }

            private static SubmitEntry InvalidSubmitEntryCommand()
            {
                var command = new SubmitEntry {Player = new SubmitEntry.PlayerClass()};
                return command;
            }

            [Fact]
            public void Response_Content_ModelState_ShouldHaveOneOrMoreProperties_WhenInvalid_SubmitEntry_Command_IsPosted()
            {
                // When
                var response = PostInvalid_SubmitEntry_Command();

                // Then
                var content = response.Content.ReadAsAsync<JObject>().Result;
                var modelState = content["modelState"];

                modelState.Count().Should().BeGreaterThan(0);
            }

            [Fact]
            public void Response_Content_ShouldHaveProperties_Message_And_ModelState_WhenInvalid_SubmitEntry_Command_IsPosted()
            {
                // When
                var response = PostInvalid_SubmitEntry_Command();

                // Then
                var content = response.Content.ReadAsAsync<JObject>().Result;
                var propertyNames = content.Properties().Select(p => p.Name);

                propertyNames.Should().BeEquivalentTo("message", "modelState");
            }

            [Fact]
            public void Response_StatusCode_ShouldBe_BadRequest_WhenInvalid_SubmitEntry_Command_IsPosted()
            {
                PostInvalid_SubmitEntry_Command()
                    .StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
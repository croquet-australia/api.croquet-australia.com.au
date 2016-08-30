using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.UnitTests.TestHelpers;
using CroquetAustralia.Library;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.Domain.UnitTests.Features.TournamentEntry.Commands
{
    public class SubmitEntryTests : TestBase
    {
        public class ToEntrySubmitted : SubmitEntryTests
        {
            public static IEnumerable<object[]> SubmitEntries => Enumerable.Range(1, 100)
                .Select(i => new[] {new ToEntrySubmitted().Valid<EntrySubmitted>()});

            [Theory]
            [MemberData(nameof(SubmitEntries))]
            public void Should_return_EntrySubmitted_event_with_populated_properties(SubmitEntry submitEntry)
            {
                // When
                var entrySubmitted = submitEntry.ToEntrySubmitted();

                // Then
                entrySubmitted.ShouldBeEquivalentTo(submitEntry, options => options.Excluding(x => x.Created));
            }
        }

        public class Validate : SubmitEntryTests
        {
            public static IEnumerable<object[]> RequiredProperties => new[]
            {
                RequiredProperty("EntityId", Guid.Empty, "EntityId is required."),
                RequiredProperty("TournamentId", Guid.Empty, "TournamentId is required."),
                RequiredProperty("Functions", null, "Functions are required."),
                RequiredProperty("Merchandise", null, "Merchandise is required."),
                RequiredProperty("Player", null, "Player is required."),
                RequiredProperty("Player.FirstName", null, "Your first name is required."),
                RequiredProperty("Player.FirstName", "", "Your first name is required."),
                RequiredProperty("Player.FirstName", " ", "Your first name is required."),
                RequiredProperty("Player.LastName", null, "Your last name is required."),
                RequiredProperty("Player.LastName", "", "Your last name is required."),
                RequiredProperty("Player.LastName", " ", "Your last name is required."),
                RequiredProperty("Player.Email", null, "Your email is required."),
                RequiredProperty("Player.Email", "", "Your email is required."),
                RequiredProperty("Player.Email", " ", "Your email is required."),
                RequiredProperty("Player.Phone", null, "Your phone is required."),
                RequiredProperty("Player.Phone", "", "Your phone is required."),
                RequiredProperty("Player.Phone", " ", "Your phone is required.")
            };

            private static object[] RequiredProperty(string property, object emptyValue, string expectedErrorMessage)
            {
                return new[] {property, emptyValue, expectedErrorMessage};
            }

            [Theory]
            [MemberData(nameof(RequiredProperties))]
            public void Should_return_ValidationResult_when_required_property_is_empty(string propertyName, object emptyValue, string expectedErrorMessage)
            {
                // Given
                var command = Invalid<SubmitEntry>(propertyName, emptyValue);
                var validationContext = new ValidationContext(command);

                // When
                try
                {
                    var validationResults = command.Validate(validationContext).ToArray();
                    var errorMessages = validationResults.Select(e => e.ErrorMessage).ToArray();

                    // Then
                    errorMessages.Should().BeEquivalentTo(expectedErrorMessage);
                }
                catch (Exception)
                {
                    ExecuteEachValidationRule(command);
                    throw;
                }
            }

            private static void ExecuteEachValidationRule(SubmitEntry command)
            {
                var validator = ServiceFactory.Get<SubmitEntryValidator>();
                var fluentValidationContext = new FluentValidation.ValidationContext(command);
                var validationRuleIndex = 0;

                foreach (var validationRule in validator)
                {
                    try
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        validationRule.Validate(fluentValidationContext).ToArray();
                        validationRuleIndex++;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"Exception thrown by validationRule '{validationRuleIndex}'.", exception);
                    }
                }
            }

            [Fact]
            public void Should_not_return_any_ValidationResult_objects_when_command_is_valid()
            {
                // Given
                var command = Valid<SubmitEntry>();
                var validationContext = new ValidationContext(command);

                // When
                var results = command.Validate(validationContext);

                // Then
                results.Should().BeEmpty();
            }
        }
    }
}
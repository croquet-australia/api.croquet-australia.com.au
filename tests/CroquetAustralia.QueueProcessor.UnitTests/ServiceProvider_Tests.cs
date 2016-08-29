using System;
using System.Collections.Generic;
using System.Linq;
using CroquetAustralia.QueueProcessor.Email;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators;
using CroquetAustralia.QueueProcessor.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.QueueProcessor.UnitTests
{
    public class ServiceProvider_Tests : TestBase
    {
        public class GetService : ServiceProvider_Tests
        {
            private readonly ServiceProvider _serviceProvider;

            static GetService()
            {
                PublicTypes = GetPublicTypes().ToArray();
            }

            public GetService()
            {
                _serviceProvider = new ServiceProvider();
            }

            public static IEnumerable<object[]> PublicTypes { get; }

            [Theory]
            [MemberData(nameof(PublicTypes))]
            public void Should_get_requested_service(Type serviceType)
            {
                _serviceProvider.GetService(serviceType).Should().NotBeNull();
            }

            private static IEnumerable<Type[]> GetPublicTypes()
            {
                var exclude = new[]
                {
                    typeof(BaseEmailGenerator),
                    typeof(EmailAddress),
                    typeof(EmailMessage),
                    typeof(MailInBlue),
                    typeof(SendEmailException),
                    typeof(TournamentEntryEmailMessage)
                };

                return typeof(Program).Assembly
                    .GetExportedTypes()
                    .Where(t => !exclude.Contains(t))
                    .Select(type => new[] {type});
            }
        }
    }
}
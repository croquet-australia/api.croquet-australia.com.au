using CroquetAustralia.Domain.Core;
using CroquetAustralia.Domain.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.Domain.UnitTests.Core
{
    public class DateTimeExtensionsTests : TestBase
    {
        public class ToZonedDateTime
        {
            [Theory]
            [InlineData("12 Mar 2016 Australia/Melbourne", "2016-03-12T00:00:00 Australia/Melbourne (+11)")]
            [InlineData("18 Feb 2016 23:59 Australia/Perth", "2016-02-18T23:59:00 Australia/Perth (+08)")]
            [InlineData("15 Mar 2016 15:00 Australia/Perth", "2016-03-15T15:00:00 Australia/Perth (+08)")]
            public void CanConvertValidDates(string text, string expected)
            {
                var zonedDateTime = text.ToZonedDateTime();

                zonedDateTime.ToString().Should().Be(expected);
            }
        }
    }
}
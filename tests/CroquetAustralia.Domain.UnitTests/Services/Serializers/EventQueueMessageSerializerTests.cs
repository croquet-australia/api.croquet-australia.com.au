﻿using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Serializers;
using CroquetAustralia.Domain.UnitTests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CroquetAustralia.Domain.UnitTests.Services.Serializers
{
    public class EventQueueMessageSerializerTests : TestBase
    {
        protected readonly QueueMessageSerializer Serializer;

        public EventQueueMessageSerializerTests()
        {
            Serializer = new EventsQueueMessageSerializer();
        }

        public class Deserialize : EventQueueMessageSerializerTests
        {
            [Fact]
            public void CanDeserializeEventQueueMessageWhenEventTypeIsEntrySubmitted()
            {
                // Arrange
                var expected = Dummy.Value<EntrySubmitted>();
                var message = Serializer.Serialize(expected);

                // Act
                var actual = Serializer.Deserialize(message);

                // Assert
                actual.ShouldBeEquivalentTo(expected);
            }
        }
    }
}
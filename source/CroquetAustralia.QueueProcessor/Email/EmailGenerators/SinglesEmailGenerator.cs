﻿using System;
using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email.EmailGenerators
{
    public class SinglesEmailGenerator : BaseEmailGenerator
    {
        public SinglesEmailGenerator(EmailMessageSettings emailMessageSettings)
            : base(emailMessageSettings)
        {
        }

        protected override string GetTemplateName(EntrySubmitted @event)
        {
            if (@event.EventId.HasValue)
            {
                return @event.Functions.Any() ? "Event and Functions" : "Event Only";
            }

            if (@event.Functions.Any())
            {
                return "Functions Only";
            }

            throw new NotSupportedException("Received an EntrySubmitted entrySubmitted where we don't know what email template to use.")
            {
                Data = {{"Event", @event}}
            };
        }
    }
}
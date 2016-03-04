using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public class SubmitEntry : EntryDto, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventId.HasValue == false && Functions.Any() == false && Merchandise.Any() == false)
            {
                yield return new ValidationResult(
                    "An entry, function or Merchandise must be selected.",
                    new[] {nameof(EventId), nameof(Functions), nameof(Merchandise)});
            }

            if (EventId.HasValue && Player.Handicap.HasValue == false)
            {
                yield return new ValidationResult(
                    "Your handicap is required.",
                    new[] {nameof(Player.Handicap)});
            }

            if (TournamentIsClosed())
            {
                yield return new ValidationResult(
                    "Entries for this tournament have closed.",
                    new[] {nameof(EventId), nameof(Functions), nameof(Merchandise)});
            }
        }

        private static bool TournamentIsClosed()
        {
            // todo: tournament related
            var closingTimeStampUtc = new DateTime(2016, 3, 4, 4, 0, 0);
            var currentTimeUtc = DateTime.UtcNow;

            return currentTimeUtc > closingTimeStampUtc;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public class SubmitEntryValidator : AbstractValidator<SubmitEntry>
    {
        private readonly ITournamentsRepository _tournaments;

        public SubmitEntryValidator(ITournamentsRepository tournaments)
        {
            _tournaments = tournaments;

            RuleFor(cmd => cmd.EntityId).NotEmpty();
            RuleFor(cmd => cmd.TournamentId).NotEmpty();
            RuleFor(cmd => cmd.TournamentId).NotEmpty().MustAsync(TournamentExistsAsync).When(cmd => cmd.TournamentId != Guid.Empty).WithMessage("Cannot find selected tournament.");
            RuleFor(cmd => cmd.EventId).NotEmpty().When(cmd => cmd.EventId.HasValue);
            RuleFor(cmd => cmd.EventId).MustAsync(EventExistsAsync).When(cmd => cmd.TournamentId != Guid.Empty && cmd.EventId.HasValue).WithMessage("Cannot find selected event.");
            RuleFor(cmd => cmd.Functions).NotNull().WithMessage("Functions are required.");
            RuleFor(cmd => cmd.Merchandise).NotNull();
            RuleFor(cmd => cmd.Player).NotNull();
            RuleFor(cmd => cmd.Player.FirstName).NotEmpty().When(cmd => cmd.Player != null).WithMessage("Your first name is required.");
            RuleFor(cmd => cmd.Player.LastName).NotEmpty().When(cmd => cmd.Player != null).WithMessage("Your last name is required.");
            RuleFor(cmd => cmd.Player.Email).NotEmpty().When(cmd => cmd.Player != null).WithMessage("Your email is required.");
            RuleFor(cmd => cmd.Player.Phone).NotEmpty().When(cmd => cmd.Player != null).WithMessage("Your phone is required.");
            RuleFor(cmd => cmd.Player.Handicap).NotEmpty().When(cmd => cmd.EventId.HasValue && cmd.Player != null && !IsGateball(cmd)).WithMessage("Your handicap is required.");
            RuleFor(cmd => cmd.Partner).NotNull().When(IsDoubles); // todo: WhenAsync is broken in 6.2.1.0
            RuleFor(cmd => cmd.Partner.FirstName).NotEmpty().When(cmd => cmd.Partner != null && IsDoubles(cmd));
            RuleFor(cmd => cmd.Partner.LastName).NotEmpty().When(cmd => cmd.Partner != null && IsDoubles(cmd));
            RuleFor(cmd => cmd.Partner.Email).NotEmpty().When(cmd => cmd.Partner != null && cmd.PayingForPartner && IsDoubles(cmd));
            RuleFor(cmd => cmd.Partner.Phone).NotEmpty().When(cmd => cmd.Partner != null && cmd.PayingForPartner && IsDoubles(cmd));
            RuleFor(cmd => cmd.Partner.Handicap).NotEmpty().When(cmd => cmd.Partner != null && cmd.PayingForPartner && IsDoubles(cmd));
            RuleFor(cmd => cmd.PaymentMethod).NotNull();

            // todo: unit test
            // todo: end to end test
            // todo: LineItems
            // throw new NotImplementedException("todo items");
        }

        private bool IsGateball(SubmitEntry command)
        {
            if (command.TournamentId == Guid.Empty)
            {
                return false;
            }

            var tournament = _tournaments.FindByIdAsync(command.TournamentId).Result;

            return tournament != null && tournament.IsGateball;
        }

        private bool IsDoubles(SubmitEntry command)
        {
            if (command.TournamentId == Guid.Empty)
            {
                return false;
            }

            var tournament = _tournaments.FindByIdAsync(command.TournamentId).Result;

            return tournament != null && tournament.IsDoubles;
        }

        private Task<bool> TournamentExistsAsync(SubmitEntry command, Guid tournamentId, CancellationToken cancellationToken)
        {
            return _tournaments.ExistsAsync(tournamentId);
        }

        private async Task<bool> EventExistsAsync(SubmitEntry command, Guid? eventId, CancellationToken cancellationToken)
        {
            if (!eventId.HasValue)
            {
                return false;
            }

            var tournament = await _tournaments.FindByIdAsync(command.TournamentId);

            return tournament != null && tournament.Events.Any(e => e.Id == eventId.Value);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailGenerator : IEmailGenerator
    {
        private readonly DoublesPartnerEmailGenerator _doublesPartnerEmailGenerator;
        private readonly DoublesPlayerEmailGenerator _doublesPlayerEmailGenerator;
        private readonly SinglesEmailGenerator _singlesEmailGenerator;
        private readonly ITournamentsRepository _tournamentsRepository;

        public EmailGenerator(
            ITournamentsRepository tournamentsRepository,
            SinglesEmailGenerator singlesEmailGenerator,
            DoublesPlayerEmailGenerator doublesPlayerEmailGenerator,
            DoublesPartnerEmailGenerator doublesPartnerEmailGenerator)
        {
            _tournamentsRepository = tournamentsRepository;
            _singlesEmailGenerator = singlesEmailGenerator;
            _doublesPlayerEmailGenerator = doublesPlayerEmailGenerator;
            _doublesPartnerEmailGenerator = doublesPartnerEmailGenerator;
        }

        public async Task<IEnumerable<EmailMessage>> GenerateAsync(EntrySubmitted entrySubmitted)
        {
            var tournament = await _tournamentsRepository.GetByIdAsync(entrySubmitted.TournamentId);
            var templateNamespace = $"CroquetAustralia.QueueProcessor.Email.Templates.PayBy{entrySubmitted.PaymentMethod}";

            if (tournament.IsDoubles)
            {
                return new[]
                {
                    _doublesPlayerEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace),
                    _doublesPartnerEmailGenerator.Generate(entrySubmitted.Partner, entrySubmitted, tournament, templateNamespace)
                };
            }

            return new[] {_singlesEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)};
        }
    }
}
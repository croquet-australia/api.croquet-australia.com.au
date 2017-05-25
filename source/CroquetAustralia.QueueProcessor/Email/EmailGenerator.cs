using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Repositories;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators;
using CroquetAustralia.QueueProcessor.Email.EmailGenerators.U21Tournament;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailGenerator : IEmailGenerator
    {
        private readonly DoublesPartnerEmailGenerator _doublesPartnerEmailGenerator;
        private readonly DoublesPlayerEmailGenerator _doublesPlayerEmailGenerator;
        private readonly GCEightsEOIEmailGenerator _gcEightsEOIEmailGenerator;
        private readonly GCWorldQualifier2017EOIEmailGenerator _gcWorldQualifier2017EOIEmailGenerator;
        private readonly Over18AndAustralianEmailGenerator _over18AndAustralianEmailGenerator;
        private readonly Over18AndNewZealanderEmailGenerator _over18AndNewZealanderEmailGenerator;
        private readonly SinglesEmailGenerator _singlesEmailGenerator;
        private readonly ITournamentsRepository _tournamentsRepository;
        private readonly U21WorldsEOIEmailGenerator _u21WorldsEOIEmailGenerator;
        private readonly Under18AndAustralianEmailGenerator _under18AndAustralianEmailGenerator;
        private readonly Under18AndNewZealanderEmailGenerator _under18AndNewZealanderEmailGenerator;
        private readonly Gateball2017EmailGenerator _gateball2017EmailGenerator;
        private readonly ACWorlds2018EOIEmailGenerator _acWorlds2018EOIEmailGenerator;

        public EmailGenerator(
            ITournamentsRepository tournamentsRepository,
            SinglesEmailGenerator singlesEmailGenerator,
            DoublesPlayerEmailGenerator doublesPlayerEmailGenerator,
            DoublesPartnerEmailGenerator doublesPartnerEmailGenerator,
            Over18AndAustralianEmailGenerator over18AndAustralianEmailGenerator,
            Under18AndAustralianEmailGenerator under18AndAustralianEmailGenerator,
            Over18AndNewZealanderEmailGenerator over18AndNewZealanderEmailGenerator,
            Under18AndNewZealanderEmailGenerator under18AndNewZealanderEmailGenerator,
            U21WorldsEOIEmailGenerator u21WorldsEOIEmailGenerator,
            GCEightsEOIEmailGenerator gcEightsEOIEmailGenerator,
            GCWorldQualifier2017EOIEmailGenerator gcWorldQualifier2017EOIEmailGenerator,
            Gateball2017EmailGenerator gateball2017EmailGenerator,
            ACWorlds2018EOIEmailGenerator acWorlds2018EOIEmailGenerator)
        {
            _tournamentsRepository = tournamentsRepository;
            _singlesEmailGenerator = singlesEmailGenerator;
            _doublesPlayerEmailGenerator = doublesPlayerEmailGenerator;
            _doublesPartnerEmailGenerator = doublesPartnerEmailGenerator;
            _over18AndAustralianEmailGenerator = over18AndAustralianEmailGenerator;
            _under18AndAustralianEmailGenerator = under18AndAustralianEmailGenerator;
            _over18AndNewZealanderEmailGenerator = over18AndNewZealanderEmailGenerator;
            _under18AndNewZealanderEmailGenerator = under18AndNewZealanderEmailGenerator;
            _u21WorldsEOIEmailGenerator = u21WorldsEOIEmailGenerator;
            _gcEightsEOIEmailGenerator = gcEightsEOIEmailGenerator;
            _gcWorldQualifier2017EOIEmailGenerator = gcWorldQualifier2017EOIEmailGenerator;
            _gateball2017EmailGenerator = gateball2017EmailGenerator;
            _acWorlds2018EOIEmailGenerator = acWorlds2018EOIEmailGenerator;
        }

        public async Task<IEnumerable<EmailMessage>> GenerateAsync(EntrySubmitted entrySubmitted)
        {
            var tournament = await _tournamentsRepository.GetByIdAsync(entrySubmitted.TournamentId);
            var templateNamespace = $"CroquetAustralia.QueueProcessor.Email.Templates.PayBy{entrySubmitted.PaymentMethod}";

            var generators = GetGenerators(entrySubmitted, tournament, templateNamespace);
            var emailMessages = generators.Select(generator => generator());

            return emailMessages;
        }

        private IEnumerable<Func<EmailMessage>> GetGenerators(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            if (tournament.IsDoubles)
            {
                return GetDoublesGenerators(entrySubmitted, tournament, templateNamespace);
            }

            if (tournament.IsUnder21)
            {
                return GetU21Generators(entrySubmitted, tournament, templateNamespace);
            }

            if (tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcEights2017EOI))
            {
                return new Func<EmailMessage>[]
                {
                    () => _gcEightsEOIEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
                };
            }

            if (tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGcWorldQualifier2017EOI))
            {
                return new Func<EmailMessage>[]
                {
                    () => _gcWorldQualifier2017EOIEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
                };
            }

            if (tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdGateballChampionships2017))
            {
                return new Func<EmailMessage>[]
                {
                    () => _gateball2017EmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
                };
            }

            if (tournament.Id == Guid.Parse(TournamentsRepository.TournamentIdAcWorldsEOI2018))
            {
                return new Func<EmailMessage>[]
                {
                    () => _acWorlds2018EOIEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
                };
            }

            return new Func<EmailMessage>[]
            {
                () => _singlesEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
            };
        }

        private IEnumerable<Func<EmailMessage>> GetU21Generators(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            if (entrySubmitted.TournamentId == Guid.Parse(TournamentsRepository.TournamentIdGcWorlds_U21_EOI_2017))
            {
                return entrySubmitted.Player.IsAgeEligible(tournament.Starts)
                    ? new Func<EmailMessage>[] {() => _u21WorldsEOIEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)}
                    : new Func<EmailMessage>[] {};
            }

            if (entrySubmitted.EventId == null)
            {
                throw new ArgumentException($"{nameof(entrySubmitted.EntityId)} cannot be null.", nameof(entrySubmitted));
            }

            // ReSharper disable once InvertIf
            if (entrySubmitted.TournamentId == Guid.Parse(TournamentsRepository.TournamentIdGcU21))
            {
                if (!tournament.PracticeStarts.HasValue)
                {
                    throw new ArgumentException($"{nameof(tournament.PracticeStarts)} cannot be null.", nameof(tournament));
                }

                // ReSharper disable once SwitchStatementMissingSomeCases because default will handle it
                switch (entrySubmitted.PaymentMethod)
                {
                    case PaymentMethod.Cash:
                        return GetNewZelanderGenerator(entrySubmitted, tournament, templateNamespace);
                    case PaymentMethod.EFT:
                        return GetAustralianGenerator(entrySubmitted, tournament, templateNamespace);
                    default:
                        throw new ArgumentException($"PaymentMethod '{entrySubmitted.PaymentMethod}' is not supported.", nameof(entrySubmitted));
                }
            }

            throw new ArgumentException($"EventId '{entrySubmitted.EventId}' is not supported.", nameof(entrySubmitted));
        }

        private IEnumerable<Func<EmailMessage>> GetNewZelanderGenerator(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            if (!tournament.PracticeStarts.HasValue)
            {
                throw new ArgumentException($"{nameof(tournament.PracticeStarts)} cannot be null.", nameof(tournament));
            }

            if (entrySubmitted.Player.IsUnder18AnytimeDuringTournamentOrPractice(tournament.PracticeStarts.Value, tournament.Starts))
            {
                yield return () => _under18AndNewZealanderEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace);
            }
            else if (entrySubmitted.Player.IsAgeEligible(tournament.Starts))
            {
                yield return () => _over18AndNewZealanderEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace);
            }
        }

        private IEnumerable<Func<EmailMessage>> GetAustralianGenerator(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            if (!tournament.PracticeStarts.HasValue)
            {
                throw new ArgumentException($"{nameof(tournament.PracticeStarts)} cannot be null.", nameof(tournament));
            }

            if (entrySubmitted.Player.IsUnder18AnytimeDuringTournamentOrPractice(tournament.PracticeStarts.Value, tournament.Starts))
            {
                yield return () => _under18AndAustralianEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace);
            }
            else if (entrySubmitted.Player.IsAgeEligible(tournament.Starts))
            {
                yield return () => _over18AndAustralianEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace);
            }
        }

        private IEnumerable<Func<EmailMessage>> GetDoublesGenerators(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            yield return () => _doublesPlayerEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace);
            yield return () => _doublesPartnerEmailGenerator.Generate(entrySubmitted.Partner, entrySubmitted, tournament, templateNamespace);
        }
    }
}
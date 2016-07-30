﻿using System;
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
        private readonly Over18AndAustralianEmailGenerator _over18AndAustralianEmailGenerator;
        private readonly Over18AndNewZealanderEmailGenerator _over18AndNewZealanderEmailGenerator;
        private readonly SinglesEmailGenerator _singlesEmailGenerator;
        private readonly ITournamentsRepository _tournamentsRepository;
        private readonly Under18AndAustralianEmailGenerator _under18AndAustralianEmailGenerator;
        private readonly Under18AndNewZealanderEmailGenerator _under18AndNewZealanderEmailGenerator;

        public EmailGenerator(
            ITournamentsRepository tournamentsRepository,
            SinglesEmailGenerator singlesEmailGenerator,
            DoublesPlayerEmailGenerator doublesPlayerEmailGenerator,
            DoublesPartnerEmailGenerator doublesPartnerEmailGenerator,
            Over18AndAustralianEmailGenerator over18AndAustralianEmailGenerator,
            Under18AndAustralianEmailGenerator under18AndAustralianEmailGenerator,
            Over18AndNewZealanderEmailGenerator over18AndNewZealanderEmailGenerator,
            Under18AndNewZealanderEmailGenerator under18AndNewZealanderEmailGenerator)
        {
            _tournamentsRepository = tournamentsRepository;
            _singlesEmailGenerator = singlesEmailGenerator;
            _doublesPlayerEmailGenerator = doublesPlayerEmailGenerator;
            _doublesPartnerEmailGenerator = doublesPartnerEmailGenerator;
            _over18AndAustralianEmailGenerator = over18AndAustralianEmailGenerator;
            _under18AndAustralianEmailGenerator = under18AndAustralianEmailGenerator;
            _over18AndNewZealanderEmailGenerator = over18AndNewZealanderEmailGenerator;
            _under18AndNewZealanderEmailGenerator = under18AndNewZealanderEmailGenerator;
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

            return new Func<EmailMessage>[]
            {
                () => _singlesEmailGenerator.Generate(entrySubmitted.Player, entrySubmitted, tournament, templateNamespace)
            };
        }

        private IEnumerable<Func<EmailMessage>> GetU21Generators(EntrySubmitted entrySubmitted, Tournament tournament, string templateNamespace)
        {
            if (entrySubmitted.EventId == null)
            {
                throw new ArgumentException("EventId cannot be null.", nameof(entrySubmitted));
            }

            // ReSharper disable once InvertIf
            if (entrySubmitted.TournamentId == Guid.Parse(TournamentsRepository.TournamentIdGcU21))
            {
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
            if (entrySubmitted.Player.IsUnder18(tournament.Starts))
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
            if (entrySubmitted.Player.IsUnder18(tournament.Starts))
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
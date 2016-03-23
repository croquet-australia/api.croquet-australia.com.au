using System;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;
using OpenMagic;
using OpenMagic.Extensions;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class DummyFactory : Dummy
    {
        private readonly ITournamentsRepository _tournamentsRepository;

        public DummyFactory(ITournamentsRepository tournamentsRepository)
        {
            _tournamentsRepository = tournamentsRepository;

            ValueFactories.Add(typeof(LineItem), LineItem);
            ValueFactories.Add(typeof(SubmitEntry), SubmitEntry);
            ValueFactories.Add(typeof(SubmitEntry.LineItem), SubmitEntryLineItem);

            // todo: what if more than 2 values
            ValueFactories.Add(typeof(PaymentMethod), () => RandomBoolean.Next() ? PaymentMethod.Cheque : PaymentMethod.EFT);
        }

        private LineItem LineItem()
        {
            var value = Object<LineItem>();

            value.DiscountPercentage = RandomNumber.NextDecimal(0, 100);
            value.Quantity = RandomNumber.NextInt(0, 100);
            value.UnitPrice = RandomNumber.NextDecimal(0, 1000);

            return value;
        }

        private SubmitEntry SubmitEntry()
        {
            var command = Object<SubmitEntry>();
            var tournament = _tournamentsRepository.GetAllAsync().Result.RandomItem();

            command.TournamentId = tournament.Id;
            command.EventId = RandomBoolean.Next() ? tournament.Events.RandomItem().Id : (Guid?)null;

            if (!command.EventId.HasValue)
            {
                command.Player.Handicap = null;
                command.PayingForPartner = false;
            }

            command.Player.Handicap = RandomNumber.NextDecimal(-3, 24);
            command.PayingForPartner = command.PayingForPartner && tournament.IsDoubles;

            if (command.PayingForPartner)
            {
                command.Partner.Handicap = RandomNumber.NextDecimal(-3, 24);
            }

            if (!tournament.IsDoubles)
            {
                command.Partner = null;
            }

            return command;
        }

        private SubmitEntry.LineItem SubmitEntryLineItem()
        {
            var value = Object<SubmitEntry.LineItem>();

            value.DiscountPercentage = RandomNumber.NextDecimal(1, 100);
            value.Quantity = RandomNumber.NextInt(1, 100);
            value.UnitPrice = RandomNumber.NextDecimal(1, 1000);

            return value;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            ValueFactories.Add(typeof(PaymentMethod), () => GetPaymentMethod() as object);
        }

        public DateTime DateOfBirth(DateTime tournamentStartDate)
        {
            var yearOfTournament = tournamentStartDate.Year;
            var oldest = new DateTime(yearOfTournament - 21, 1, 1);
            var youngest = new DateTime(yearOfTournament - 1, 1, 1);

            return RandomDateTime.Enumerable(1, oldest, youngest).Single();
        }

        protected override IEnumerable CreateValues(Type itemType)
        {
            return itemType == typeof(SubmitEntry.LineItem) ? CreateValues(itemType, RandomNumber.NextInt(0, 4)) : base.CreateValues(itemType);
        }

        private LineItem LineItem()
        {
            var value = Object<LineItem>();

            value.DiscountPercentage = RandomNumber.NextDecimal(0, 100);
            value.Quantity = RandomNumber.NextInt(0, 100);
            value.UnitPrice = RandomNumber.NextDecimal(0, 1000);

            return value;
        }

        private T Object<T>()
        {
            return (T)base.Object(typeof(T));
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

            // todo: Remove this to allow pay by cash
            if (tournament.IsUnder21)
            {
                command.Player.DateOfBirth = DateOfBirth(tournament.Starts.ToDateTimeUtc());
                command.Player.NonResident = RandomBoolean.Next();
                command.PaymentMethod = command.Player.NonResident.Value ? PaymentMethod.Cash : PaymentMethod.EFT;
                command.Functions = new SubmitEntry.LineItem[] {};
                command.Merchandise = new SubmitEntry.LineItem[] {};
            }
            else
            {
                command.Player.DateOfBirth = null;
                command.Player.NonResident = null;
                command.PaymentMethod = GetPaymentMethod(paymentMethod => paymentMethod != PaymentMethod.Cash);
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

        private static PaymentMethod GetPaymentMethod()
        {
            return GetPaymentMethod(paymentMethod => true);
        }

        private static PaymentMethod GetPaymentMethod(Func<PaymentMethod, bool> predicate)
        {
            return GetPaymentMethods().Where(predicate).RandomItem();
        }

        private static IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return Enum.GetValues(typeof(PaymentMethod))
                .Cast<PaymentMethod>();
        }
    }
}
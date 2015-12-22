using System;
using System.Collections.Generic;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;
using OpenMagic;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class TestBase
    {
        protected IDummy Dummy;

        public TestBase()
        {
            Dummy = new DummyFactory();
        }
    }

    public class DummyFactory : Dummy
    {
        public DummyFactory()
        {
            ValueFactories.Add(typeof(LineItem), DummyLineItem);

            // todo: what if more than 2 values
            ValueFactories.Add(typeof(PaymentMethod), () => RandomBoolean.Next() ? PaymentMethod.Cheque: PaymentMethod.EFT);
        }

        private LineItem DummyLineItem()
        {
            var value = Object<LineItem>();

            value.DiscountPercentage = RandomNumber.NextDecimal(0, 100);
            value.Quantity = RandomNumber.NextInt(0, 100);
            value.UnitPrice = RandomNumber.NextDecimal(0, 1000);

            return value;
        }
    }
}
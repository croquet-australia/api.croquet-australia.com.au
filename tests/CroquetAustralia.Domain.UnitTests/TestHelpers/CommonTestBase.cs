using System;
using System.Text;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Models;
using CroquetAustralia.Library.Extensions;
using OpenMagic;
using OpenMagic.Extensions;

namespace CroquetAustralia.Domain.UnitTests.TestHelpers
{
    public class CommonTestBase
    {
        protected IDummy Dummy;
        protected Exception CaughtException;

        protected CommonTestBase()
        {
            Dummy = new DummyFactory();
        }

        protected void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (AggregateException exception)
            {
                throw new Exception("Aggregate Exception", exception.InnerException);
            }
        }
    }

    public class DummyFactory : Dummy
    {
        public DummyFactory()
        {
            ValueFactories.Add(typeof (LineItem), DummyLineItem);

            // todo: what if more than 2 values
            ValueFactories.Add(typeof (PaymentMethod), () => RandomBoolean.Next() ? PaymentMethod.Cheque : PaymentMethod.EFT);
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

    public static class Extensions
    {
        public static string ToApprovalText(this object value)
        {
            var yaml = value.ToYaml();
            var formatted = FormatMultiLineYamlFields(yaml);
            
            return formatted;
        }

        private static string FormatMultiLineYamlFields(string yaml)
        {
            var lines = yaml.ToLines();
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                // Multiline yaml fields have the following format:
                // PropertyName: "value"

                // if the line doesn't end with " then it isn't a multiline field.
                if (!line.EndsWith("\""))
                {
                    sb.AppendLine(line);
                    continue;
                }

                var endOfPropertyName = line.IndexOf(": ", StringComparison.Ordinal);

                // I don't expect this if to ever by true.
                if (endOfPropertyName == -1)
                {
                    sb.AppendLine(line);
                    continue;
                }

                var value = line.Substring(endOfPropertyName + 2);

                // if the value doesn't start with " then it isn't a multiline field.
                if (!value.StartsWith("\""))
                {
                    sb.AppendLine(line);
                    continue;
                }

                var propertyName = line.Substring(0, endOfPropertyName + 2).Trim();
                value = value.Trim('"').Replace(@"\r\n", @"\n").Replace(@"\n", Environment.NewLine).Replace(@"\""", @"""");
                
                sb.AppendLine(propertyName);
                sb.AppendLine(value);
            }

            return sb.ToString();
        }
    }
}
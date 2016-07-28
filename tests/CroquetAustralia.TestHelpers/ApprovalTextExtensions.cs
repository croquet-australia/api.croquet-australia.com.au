using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMagic.Extensions;
using YamlDotNet.Serialization;

namespace CroquetAustralia.TestHelpers
{
    public static class ApprovalTextExtensions
    {
        public static string ToApprovalText(this object value)
        {
            return value.ToApprovalText(Enumerable.Empty<IYamlTypeConverter>());
        }

        public static string ToApprovalText(this object value, IEnumerable<IYamlTypeConverter> typeConverters)
        {
            var yaml = value.ToYaml(typeConverters);
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
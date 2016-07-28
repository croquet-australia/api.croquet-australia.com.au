using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CroquetAustralia.QueueProcessor.Email;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace CroquetAustralia.QueueProcessor.UnitTests.TestHelpers
{
    public class InMemoryAttachmentsConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(EmailAttachments);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            throw new NotImplementedException();
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var files = (IEnumerable<FileInfo>)value;
            var write = string.Join(",", files.Select(file => file.Name));
            var parsingEvent = new Scalar(write);

            emitter.Emit(parsingEvent);
        }
    }
}
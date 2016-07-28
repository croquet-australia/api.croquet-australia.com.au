using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.QueueProcessor.Email
{
    public class SendEmailException : Exception
    {
        private readonly Dictionary<string, object> _data;

        internal SendEmailException(JToken code, JToken message, JToken data, EmailMessage emailMessage)
            : base($"Could not send '{emailMessage.Subject}' to '{emailMessage.To.First().Name} <{emailMessage.To.First().Email}>'")
        {
            _data = new Dictionary<string, object>
            {
                {"code", code},
                {"message", message},
                {"data", data},
                {"emailMessage", emailMessage}
            };
        }

        public override IDictionary Data => _data;
    }
}
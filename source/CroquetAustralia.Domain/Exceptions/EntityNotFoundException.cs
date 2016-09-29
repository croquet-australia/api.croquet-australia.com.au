using System;
using System.Collections;
using System.Collections.Generic;

namespace CroquetAustralia.Domain.Exceptions
{
    public abstract class EntityNotFoundException : Exception
    {
        private Dictionary<string, object> _data;

        protected EntityNotFoundException(string where, string type)
            : base(CreateMessage(type))
        {
            Init(where);
        }

        protected EntityNotFoundException(string where, string type, Exception innerException) 
            : base(CreateMessage(type), innerException)
        {
            Init(where);
        }

        public override IDictionary Data => _data;

        private void Init(string @where)
        {
            _data = new Dictionary<string, object>
            {
                {"where", @where}
            };
        }

        private static string CreateMessage(string type)
        {
            return $"Cannot find requested {type}.";
        }
    }
}
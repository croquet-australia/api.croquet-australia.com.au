using System;
using System.Collections;
using System.Collections.Generic;

namespace CroquetAustralia.Domain.Exceptions
{
    public abstract class EntityNotFoundException : Exception
    {
        private readonly Dictionary<string, object> _data;

        protected EntityNotFoundException(string where, string type)
            : base($"Cannot find requested {type}.")
        {
            _data = new Dictionary<string, object>
            {
                {"where", where}
            };
        }

        public override IDictionary Data => _data;
    }
}
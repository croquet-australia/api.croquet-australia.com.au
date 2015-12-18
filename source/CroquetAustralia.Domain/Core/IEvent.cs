using System;

namespace CroquetAustralia.Domain.Core
{
    public interface IEvent
    {
        Guid Id { get; set; }
    }
}
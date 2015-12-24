using System;

namespace CroquetAustralia.Domain.Core
{
    public interface IEvent
    {
        Guid EntityId { get; }
        DateTime Created { get; }
    }
}
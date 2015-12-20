using System;

namespace CroquetAustralia.Domain.Core
{
    public interface IEvent
    {
        Guid EntityId { get; set; }
        DateTime Created { get; set; }
    }
}
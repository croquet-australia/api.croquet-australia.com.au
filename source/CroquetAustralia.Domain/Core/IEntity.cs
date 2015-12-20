using System;

namespace CroquetAustralia.Domain.Core
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
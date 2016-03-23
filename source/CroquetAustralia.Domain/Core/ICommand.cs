using System;
using System.ComponentModel.DataAnnotations;

namespace CroquetAustralia.Domain.Core
{
    public interface ICommand : IValidatableObject
    {
        Guid EntityId { get; set; }
    }
}
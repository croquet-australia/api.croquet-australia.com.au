using System;

namespace CroquetAustralia.Domain.Core
{
    public interface ICommand
    {
        Guid Id { get; set; }
    }
}
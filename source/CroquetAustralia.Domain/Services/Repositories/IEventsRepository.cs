using System;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Core;

namespace CroquetAustralia.Domain.Services.Repositories
{
    public interface IEventsRepository
    {
        Task AddAsync(IEvent @event);
    }
}
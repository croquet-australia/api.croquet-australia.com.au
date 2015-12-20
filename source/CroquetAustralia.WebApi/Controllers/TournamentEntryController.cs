using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services;

namespace CroquetAustralia.WebApi.Controllers
{
    [RoutePrefix("tournament-entry")]
    public class TournamentEntryController : ApiController
    {
        private readonly IEventQueue _eventQueue;

        public TournamentEntryController(IEventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        [HttpPost, Route("add-entry")]
        public async Task AddEntryAsync(SubmitEntry command)
        {
            // todo: extension method command.MapTo<EntrySubmitted>
            var @event = Mapper.Map<EntrySubmitted>(command);

            await _eventQueue.AddEventAsync(@event);
        }
    }
}
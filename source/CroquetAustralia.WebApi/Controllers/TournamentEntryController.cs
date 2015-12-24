using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CroquetAustralia.Domain.Features.TournamentEntry;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;
using CroquetAustralia.Domain.Services.Queues;

namespace CroquetAustralia.WebApi.Controllers
{
    [RoutePrefix("tournament-entry")]
    public class TournamentEntryController : ApiController
    {
        private readonly IEventsQueue _eventsQueue;

        public TournamentEntryController(IEventsQueue eventsQueue)
        {
            _eventsQueue = eventsQueue;
        }

        [HttpPost, Route("add-entry")]
        public async Task AddEntryAsync(SubmitEntry command)
        {
            // todo: extension method command.MapTo<EntrySubmitted>
            var @event = Mapper.Map<EntrySubmitted>(command);

            await _eventsQueue.AddMessageAsync(@event);
        }

        [HttpPost, Route("payment-received")]
        public async Task PaymentReceivedAsync(ReceivePayment command)
        {
            // todo: extension method command.MapTo<EntrySubmitted>
            var @event = new PaymentReceived(command.EntityId, command.PaymentMethod);

            await _eventsQueue.AddMessageAsync(@event);
        }
    }
}
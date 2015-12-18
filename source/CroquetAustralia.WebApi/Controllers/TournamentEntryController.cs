using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        public Task AddEntryAsync(SubmitEntry command)
        {
                throw new NotImplementedException("Model is valid");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using CroquetAustralia.Domain.Data;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;

namespace CroquetAustralia.WebApi.Controllers
{
    [RoutePrefix("tournaments")]
    public class TournamentsController : ApiController
    {
        private readonly ITournamentsRepository _tournamentsRepository;

        public TournamentsController(ITournamentsRepository tournamentsRepository)
        {
            _tournamentsRepository = tournamentsRepository;
        }

        [Route("")]
        public Task<IEnumerable<Tournament>> GetAllTournaments()
        {
            return _tournamentsRepository.GetAllAsync();
        }

        [Route("{tournamentId}")]
        public Task<Tournament> GetTournamentById(Guid tournamentId)
        {
            return _tournamentsRepository.GetByIdAsync(tournamentId);
        }

        [Route("{year}/{discipline}/{slug}")]
        public Task<Tournament> GetTournamentBySlug(int year, string discipline, string slug)
        {
            return _tournamentsRepository.GetBySlugAsync(year, discipline, slug);
        }
    }
}
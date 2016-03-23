using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Commands
{
    public interface ITournamentsRepository
    {
        Task<bool> ExistsAsync(Guid tournamentId);
        Task<Tournament> FindByIdAsync(Guid tournamentId);
        Task<IEnumerable<Tournament>> GetAllAsync();
        Task<Tournament> GetByIdAsync(Guid tournamentId);
        Task<Tournament> GetBySlugAsync(int year, string discipline, string slug);
        Task<Tournament> GetByUrlAsync(string url);
    }
}
using System;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.Domain.Exceptions
{
    public class TournamentNotFoundException : EntityNotFoundException
    {
        internal TournamentNotFoundException(Func<Tournament, bool> where)
            : base(where.ToString(), "tournament")
        {
        }
    }
}
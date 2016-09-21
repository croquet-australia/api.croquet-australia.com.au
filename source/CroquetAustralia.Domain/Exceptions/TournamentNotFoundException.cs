using System;
using CroquetAustralia.Domain.Data;

namespace CroquetAustralia.Domain.Exceptions
{
    public class TournamentNotFoundException : EntityNotFoundException
    {
        internal TournamentNotFoundException(Func<Tournament, bool> where)
            : this(where.ToString())
        {
        }

        internal TournamentNotFoundException(string where)
            : base(where, "tournament")
        {
        }

        internal TournamentNotFoundException(string where, Exception innerException)
            : base(where, "tournament", innerException)
        {
        }
    }
}
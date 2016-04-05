using CroquetAustralia.Domain.Features.TournamentEntry.Commands;

namespace CroquetAustralia.DownloadTournamentEntries.ReadModels
{
    public class PlayerReadModel : SubmitEntry.PlayerClass
    {
        public PlayerReadModel(SubmitEntry.PlayerClass source)
        {
            FirstName = source.FirstName;
            Email = source.Email;
            FullTimeStudentUnder25 = source.FullTimeStudentUnder25;
            Handicap = source.Handicap;
            LastName = source.LastName;
            Phone = source.Phone;
            Under21 = source.Under21;
        }
    }
}
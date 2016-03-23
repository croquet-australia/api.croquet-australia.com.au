using System.Collections.Generic;
using System.Threading.Tasks;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.QueueProcessor.Email
{
    public interface IEmailGenerator
    {
        Task<IEnumerable<EmailMessage>> GenerateAsync(EntrySubmitted entrySubmitted);
    }
}
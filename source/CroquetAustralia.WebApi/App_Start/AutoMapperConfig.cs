using AutoMapper;
using CroquetAustralia.Domain.Features.TournamentEntry.Commands;
using CroquetAustralia.Domain.Features.TournamentEntry.Events;

namespace CroquetAustralia.WebApi
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<SubmitEntry, EntrySubmitted>());
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Anotar.NLog;
using Newtonsoft.Json.Linq;

namespace CroquetAustralia.WebApi.Controllers
{
    [RoutePrefix("tournament-entry")]
    public class TournamentEntryController : ApiController
    {
        [HttpPost, Route("add-entry")]
        public Task AddEntry(JObject entry)
        {
            LogTo.Error("todo: TournamentEntryController.AddEntry(JObject entry)");
            return Task.FromResult(0);
        }
    }
}
using System.Reflection;
using System.Runtime.CompilerServices;
using Anotar.NLog;

[assembly: AssemblyTitle("CroquetAustralia.Domain")]
[assembly: AssemblyDescription("Croquet Australia's domain layer")]
[assembly: LogMinimalMessage]
[assembly: InternalsVisibleTo("CroquetAustralia.Domain.Specifications")]
[assembly: InternalsVisibleTo("CroquetAustralia.Domain.UnitTests")]
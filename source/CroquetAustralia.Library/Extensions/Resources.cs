using System;
using System.IO;
using System.Reflection;

namespace CroquetAustralia.Library.Extensions
{
    public static class Resources
    {
        public static string GetResourceText(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception($"Cannot find resource '{resourceName}' in assembly '{assembly.GetName()}'\n\n{string.Join("\n", assembly.GetManifestResourceNames())}.");
                }
                using (var reader = new StreamReader(stream))
                {
                    var template = reader.ReadToEnd();

                    return template;
                }
            }
        }
    }
}
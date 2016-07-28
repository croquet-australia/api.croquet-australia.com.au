using System;
using System.IO;
using System.Reflection;

namespace CroquetAustralia.Library.Extensions
{
    public static class Resources
    {
        public static string GetResourceText(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var template = reader.ReadToEnd();

                    return template;
                }
            }
        }

        public static void SaveResourceAsFile(this Assembly assembly, string resourceName, FileInfo saveAs)
        {
            using (var resourceStream = assembly.GetResourceStream(resourceName))
            using (var fileStream = File.Create(saveAs.FullName))
            {
                resourceStream.CopyTo(fileStream);
            }
        }

        public static Stream GetResourceStream(this Assembly assembly, string resourceName)
        {
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new ArgumentException($"Cannot find resource '{resourceName}' in assembly '{assembly.GetName()}'\n\n{string.Join("\n", assembly.GetManifestResourceNames())}.", nameof(resourceName));
            }

            return stream;
        }
    }
}
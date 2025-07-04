using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCLI.Helpers
{
    public static class ApiKeyStore
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "weathercli", "apikey.txt");

        public static void SaveApiKey(string key)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, key);
        }

        public static string? GetApiKey()
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath) : null;
        }
    }
}

using System.CommandLine;
using WeatherCLI.Helpers;

namespace WeatherCLI.Commands
{
    public class LoginCommand
    {
        public static Command Create()
        {
            var keyArg = new Argument<string>("apikey", "Your API key to store");

            var command = new Command("login", "Store your API key securely for future use")
        {
            keyArg
        };

            command.SetHandler((string apikey) =>
            {
                ApiKeyStore.SaveApiKey(apikey);
                Console.WriteLine("🔐 API key saved successfully.");
            }, keyArg);

            return command;
        }
    }
}

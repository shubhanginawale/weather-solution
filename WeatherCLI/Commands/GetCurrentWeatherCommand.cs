using System.CommandLine;
using WeatherCLI.Helpers;
using WeatherCLI.Services;

namespace WeatherCLI.Commands
{
    public class GetCurrentWeatherCommand
    {
        public static Command Create()
        {
            var zipArg = new Argument<string>("zip");
            var unitsArg = new Argument<string>("units");
            var outputOpt = new Option<string>("--output", () => "text", "Output format: text (default), json, yaml");
            var apikeyOpt = new Option<string>("--apikey", "API key to authenticate");

            var command = new Command("get-current-weather", "Get current weather by ZIP code")
            {
                 zipArg,
                 unitsArg,
                 outputOpt,
                 apikeyOpt
            };

            command.SetHandler(async (string zip, string units, string output, string apikey) =>
            {
                var client = new WeatherApiClient(apikey);
                var weather = await client.GetCurrentWeatherAsync(zip, units);
                OutputFormatter.Print(weather, output,zip);
            },
            zipArg, unitsArg, outputOpt, apikeyOpt);

            return command;
        }
    }
}

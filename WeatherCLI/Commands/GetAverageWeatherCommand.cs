using System.CommandLine;
using WeatherCLI.Services;
using WeatherCLI.Helpers;

namespace WeatherCLI.Commands
{
    public static class GetAverageWeatherCommand
    {
        public static Command Create()
        {
            var zipArg = new Argument<string>("zip");
            var unitsArg = new Argument<string>("units");
            var daysArg = new Argument<int>("days");
            var outputOpt = new Option<string>("--output", () => "text", "Output format: text (default), json, yaml");
            var apikeyOpt = new Option<string>("--apikey", "API key to authenticate");

            var command = new Command("get-average-weather", "Get average weather over days")
        {
            zipArg,
            unitsArg,
            daysArg,
            outputOpt,
            apikeyOpt
        };

            command.SetHandler(async (string zip, string units, int days, string output, string apikey) =>
            {
                var client = new WeatherApiClient(apikey);
                var weather = await client.GetAverageWeatherAsync(zip, units, days);
                OutputFormatter.Print(weather, output,zip);
            }, zipArg, unitsArg, daysArg, outputOpt, apikeyOpt);

            return command;
        }
    }
}

using System.CommandLine;
using WeatherCLI.Commands;

var rootCommand = new RootCommand("Weather CLI")
{
    GetCurrentWeatherCommand.Create(),
    GetAverageWeatherCommand.Create(),
    LoginCommand.Create() 

};

return await rootCommand.InvokeAsync(args);
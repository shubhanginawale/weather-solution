using System.Text.Json;
using WeatherCLI.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WeatherCLI.Helpers
{
    public class OutputFormatter
    {
        public static void Print(CurrentWeatherResponse data, string format)
        {
            switch (format.ToLower())
            {
                case "json":
                    Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
                    break;

                case "yaml":
                    var yaml = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    Console.WriteLine(yaml.Serialize(data));
                    break;

                case "text":
                default:
                    
                    Console.WriteLine($"Location: {data.ZipCode}");
                    Console.WriteLine($"Current Temperature: {data.CurrentTemperature}{data.Unit}");
                    Console.WriteLine($"Rain Possible Today: {(data.RainPossibleToday ? "Yes" : "No")}");
                    break;
            }
        }
        public static void Print(AverageWeatherResponse data, string format)
        {
            switch (format.ToLower())
            {
                case "json":
                    Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
                    break;

                case "yaml":
                    var yaml = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    Console.WriteLine(yaml.Serialize(data));
                    break;

                case "text":
                default:
                    
                    Console.WriteLine($"Location: {data.ZipCode}");
                    Console.WriteLine($"Average Temperature: {data.AverageTemperature}{data.Unit}");
                    Console.WriteLine($"Rain Expected During Period: {(data.RainPossibleInPeriod ? "Yes" : "No")}");
                    break;
            }
        }
    }
}

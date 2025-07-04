using System.Text.Json;
using WeatherCLI.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WeatherCLI.Helpers
{
    public class OutputFormatter
    {
        public static void Print(WeatherResult data, string outputFormat)
        {
            switch (outputFormat.ToLower())
            {
                case "json":
                    Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
                    break;

                case "yaml":
                    var serializer = new SerializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    Console.WriteLine(serializer.Serialize(data));
                    break;

                case "text":
                default:
                    Console.WriteLine($"Location: {data.Latitude},{data.Longitude}");
                    Console.WriteLine($"Temperature: {data.Temperature} {data.Unit}");
                    Console.WriteLine($"Rain Expected: {(data.RainExpected ? "Yes" : "No")}");
                    break;
            }
        }
    }
}

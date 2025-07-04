using System.Net;
using System.Text.Json;
using WeatherService.Exceptions;
using WeatherService.Models;
using WeatherService.Services.Interfaces;

namespace WeatherService.Services
{
    public class WeatherProvider : IWeatherProvider
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private const string Fahrenheit = "fahrenheit";

        public WeatherProvider(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeatherMap:ApiKey"];
            _baseUrl = config["OpenWeatherMap:BaseUrl"];
        }

        public async Task<WeatherResult> GetCurrentWeatherAsync(string zipCode, string unit)
        {
            var unitParam = unit.ToLower() == Fahrenheit ? "imperial" : "metric";
            var response = await _httpClient.GetAsync(
                $"{_baseUrl}/data/2.5/weather?zip={zipCode}&units={unitParam}&appid={_apiKey}");

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound )
                throw new LocationNotFoundException("Location not found");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return ParseCurrentWeatherSafe(root, unit);
        }

        public async Task<WeatherResult> GetAverageWeatherAsync(string zipCode, string unit, int days)
        {

            var unitParam = unit.ToLower() == Fahrenheit ? "imperial" : "metric";
            var response = await _httpClient.GetAsync(
                $"{_baseUrl}/data/2.5/forecast?zip={zipCode}&units={unitParam}&cnt={days * 8}&appid={_apiKey}");

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
                throw new LocationNotFoundException("Location not found");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return ParseAverageWeatherSafe(root, unit);

        }

        private WeatherResult ParseCurrentWeatherSafe(JsonElement root, string unit)
        {
            float temp = 0, lat = 0, lon = 0;
            bool rainExpected = false;

            if (root.TryGetProperty("main", out var mainProp) &&
                mainProp.TryGetProperty("temp", out var tempProp) &&
                tempProp.TryGetSingle(out var t))
            {
                temp = t;
            }

            if (root.TryGetProperty("coord", out var coordProp))
            {
                if (coordProp.TryGetProperty("lat", out var latProp) &&
                    latProp.TryGetSingle(out var latitude))
                    lat = latitude;

                if (coordProp.TryGetProperty("lon", out var lonProp) &&
                    lonProp.TryGetSingle(out var longitude))
                    lon = longitude;
            }

            if (root.TryGetProperty("rain", out _))
                rainExpected = true;
            else if (root.TryGetProperty("weather", out var weatherProp) &&
                     weatherProp.ValueKind == JsonValueKind.Array &&
                     weatherProp.GetArrayLength() > 0 &&
                     weatherProp[0].TryGetProperty("main", out var mainWeatherProp))
            {
                var mainWeatherStr = mainWeatherProp.GetString();
                if (!string.IsNullOrEmpty(mainWeatherStr) && mainWeatherStr.ToLower().Contains("rain"))
                    rainExpected = true;
            }

            return new WeatherResult
            {
                Temperature = temp,
                Unit = unit.ToLower() == "fahrenheit" ? "F" : "C",
                Latitude = lat,
                Longitude = lon,
                RainExpected = rainExpected
            };
        }

        private WeatherResult ParseAverageWeatherSafe(JsonElement root, string unit)
        {
            var temps = root.TryGetProperty("list", out var listProp) && listProp.ValueKind == JsonValueKind.Array
                ? listProp.EnumerateArray()
                    .Select(x => x.TryGetProperty("main", out var mainProp) &&
                                 mainProp.TryGetProperty("temp", out var tempProp) &&
                                 tempProp.TryGetSingle(out var val) ? val : (float?)null)
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                : Enumerable.Empty<float>();

            float avgTemp = temps.Any() ? temps.Average() : 0;

            float lat = 0, lon = 0;
            if (root.TryGetProperty("city", out var cityProp) &&
                cityProp.TryGetProperty("coord", out var coordProp))
            {
                if (coordProp.TryGetProperty("lat", out var latProp) &&
                    latProp.TryGetSingle(out var latitude))
                    lat = latitude;

                if (coordProp.TryGetProperty("lon", out var lonProp) &&
                    lonProp.TryGetSingle(out var longitude))
                    lon = longitude;
            }

            bool rainExpected = root.TryGetProperty("list", out var weatherListProp) && weatherListProp.ValueKind == JsonValueKind.Array &&
                                weatherListProp.EnumerateArray().Any(x => x.TryGetProperty("rain", out _));

            return new WeatherResult
            {
                Temperature = avgTemp,
                Unit = unit.ToLower() == "fahrenheit" ? "F" : "C",
                Latitude = lat,
                Longitude = lon,
                RainExpected = rainExpected
            };
        }
    }  
   
}

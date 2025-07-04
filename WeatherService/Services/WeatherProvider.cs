using System.Text.Json;
using WeatherService.Models;
using WeatherService.Services.Interfaces;

namespace WeatherService.Services
{
    public class WeatherProvider : IWeatherProvider
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherProvider(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeatherMap:ApiKey"];
        }

        public async Task<WeatherResult> GetCurrentWeatherAsync(string zipCode, string unit)
        {
            var unitParam = unit.ToLower() == "fahrenheit" ? "imperial" : "metric";
            var response = await _httpClient.GetAsync(
                $"https://api.openweathermap.org/data/2.5/weather?zip={zipCode}&units={unitParam}&appid={_apiKey}");

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException("Invalid zip code");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new WeatherResult
            {
                Temperature = root.GetProperty("main").GetProperty("temp").GetSingle(),
                Unit = unit.ToLower() == "fahrenheit" ? "F" : "C",
                Latitude = root.GetProperty("coord").GetProperty("lat").GetSingle(),
                Longitude = root.GetProperty("coord").GetProperty("lon").GetSingle(),
                RainExpected = root.TryGetProperty("rain", out _) ||
                                root.GetProperty("weather")[0].GetProperty("main").GetString()?.ToLower().Contains("rain") == true
            };
        }

        public async Task<WeatherResult> GetAverageWeatherAsync(string zipCode, string unit, int days)
        {
            if (days < 2 || days > 5) throw new ArgumentException("timePeriod must be 2–5");

            var unitParam = unit.ToLower() == "fahrenheit" ? "imperial" : "metric";
            var response = await _httpClient.GetAsync(
                $"https://api.openweathermap.org/data/2.5/forecast?zip={zipCode}&units={unitParam}&cnt={days * 8}&appid={_apiKey}");

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException("Invalid zip code");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var temps = root.GetProperty("list").EnumerateArray().Select(x => x.GetProperty("main").GetProperty("temp").GetSingle());
            var avgTemp = temps.Average();

            var rain = root.GetProperty("list").EnumerateArray().Any(x => x.TryGetProperty("rain", out _));

            return new WeatherResult
            {
                Temperature = avgTemp,
                Unit = unit.ToLower() == "fahrenheit" ? "F" : "C",
                Latitude = root.GetProperty("city").GetProperty("coord").GetProperty("lat").GetSingle(),
                Longitude = root.GetProperty("city").GetProperty("coord").GetProperty("lon").GetSingle(),
                RainExpected = rain
            };
        }

        //public async Task<AverageWeatherResponse> GetAverageWeatherAsync(string zipcode, string units, int timePeriod)
        //{
        //    if (timePeriod < 2 || timePeriod > 5)
        //        throw new ArgumentException("timePeriod must be 2-5 days.");

        //    // Fetch forecast data
        //    var forecastData = await _clientService.GetForecastDataAsync(zipcode, units);

        //    // Filter data for the next `timePeriod` days
        //    var endDate = DateTime.UtcNow.AddDays(timePeriod);
        //    var relevantForecasts = forecastData.Forecasts
        //        .Where(f => f.DateTime <= endDate)
        //        .ToList();

        //    if (!relevantForecasts.Any())
        //        throw new InvalidOperationException("No forecast data available for the period.");

        //    // Calculate averages
        //    double avgTemp = relevantForecasts.Average(f => f.Main.Temp);
        //    bool rainPossible = relevantForecasts.Any(f =>
        //        f.Weather.Any(w => w.Main == "Rain"));

        //    return new AverageWeatherResponse
        //    {
        //        AverageTemperature = avgTemp,
        //        Unit = units == "fahrenheit" ? "F" : "C",
        //        Lat = forecastData.City?.Coord.Lat ?? 0,  // Assume City is part of the response
        //        Lon = forecastData.City?.Coord.Lon ?? 0,
        //        RainPossibleInPeriod = rainPossible
        //    };
        //};
        //}

        //public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string zipcode, string units)
        //{
        //    var data = await _clientService.GetWeatherDataAsync(zipcode, units);

        //    return new CurrentWeatherResponse
        //    {
        //        CurrentTemperature = data.Main.Temp,
        //        Unit = units == "fahrenheit" ? "F" : "C",
        //        Lat = data.Coord.Lat,
        //        Lon = data.Coord.Lon,
        //        RainPossibleToday = data.Weather.Any(w => w.Main == "Rain")
        //    };
        //}

    }  
   
}

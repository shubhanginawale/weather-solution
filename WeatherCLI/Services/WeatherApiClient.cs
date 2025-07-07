using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherCLI.Models;

namespace WeatherCLI.Services
{
    public class WeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/weather/";

        public WeatherApiClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string zip, string units)
        {
            string unitQuery = units.ToLower() switch
            {
                "fahrenheit" => "imperial",
                _ => "metric"
            };
            var response = await _httpClient.GetAsync($"{BaseUrl}current/{zip}?units={units}");
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CurrentWeatherResponse>() ?? throw new Exception("Empty response");
            result.ZipCode = zip;
            return result;
            
        }

        public async Task<AverageWeatherResponse> GetAverageWeatherAsync(string zip, string units, int days)
        {
            string unitQuery = units.ToLower() switch
            {
                "fahrenheit" => "imperial",
                _ => "metric"
            };
            var response = await _httpClient.GetAsync($"{BaseUrl}average/{zip}?units={units}&timePeriod={days}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AverageWeatherResponse>() ?? throw new Exception("Empty response");
            result.ZipCode = zip;
            return result;
            
        }
    }
}

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

        public async Task<WeatherResult> GetCurrentWeatherAsync(string zip, string units)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}current/{zip}?units={units}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WeatherResult>() ?? throw new Exception("Empty response");
        }

        public async Task<WeatherResult> GetAverageWeatherAsync(string zip, string units, int days)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}average/{zip}?units={units}&timePeriod={days}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WeatherResult>() ?? throw new Exception("Empty response");
        }
    }
}

using Xunit;
using System.Threading.Tasks;
using WeatherService.Services;
using WeatherService.Exceptions; // For LocationNotFoundException
using Microsoft.Extensions.Configuration;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace WeatherService.Tests.Services
{
    public class WeatherProviderTests
    {
        private readonly IConfiguration _config;
        public WeatherProviderTests()
        {
            var inMemorySettings = new Dictionary<string, string>
        {
            { "OpenWeatherMap:ApiKey", "mock-api-key" },
            { "OpenWeatherMap:BaseUrl", "https://api.openweathermap.org" }
        };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        [Fact]
        public async Task GetCurrentWeatherAsync_ValidZip_ReturnsWeatherResult()
        {
            var mockHttp = new MockHttpMessageHandler();

            string responseJson = """
        {
          "coord": { "lon": -71.316711 , "lat": 42.640998840332031 },
          "weather": [ { "main": "Clear" } ],
          "main": { "temp": 87 },
          "name": "Lowell"
        }
        """;

            mockHttp.When("https://api.openweathermap.org/data/2.5/weather*")
                    .Respond("application/json", responseJson);

            var provider = new WeatherProvider(new HttpClient(mockHttp), _config);

            var result = await provider.GetCurrentWeatherAsync("01854", "fahrenheit");

            Assert.Equal(87, result.Temperature);
            Assert.Equal("F", result.Unit);
            Assert.Equal(42.640998f, result.Latitude);
            Assert.Equal(-71.316711f, result.Longitude);
            Assert.False(result.RainExpected);
        }
        [Fact]
        public async Task GetAverageWeatherAsync_Valid_ReturnsAverage()
        {
            var json = """
        {
          "city": { "coord": { "lat": 1, "lon": 2 } },
          "list": [
            { "main": { "temp": 10.0 } },
            { "main": { "temp": 20.0 }, "rain": { "3h": 1.0 } }
          ]
        }
        """;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.openweathermap.org/data/2.5/forecast*").Respond("application/json", json);

            var service = new WeatherProvider(new HttpClient(mockHttp), _config);

            var result = await service.GetAverageWeatherAsync("01720", "fahrenheit", 2);

            Assert.Equal(15, result.Temperature);
            Assert.Equal("F", result.Unit);
            Assert.True(result.RainExpected);
        }
        [Fact]
        public async Task GetCurrentWeatherAsync_InvalidZip_ThrowsLocationNotFoundException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*weather*").Respond(System.Net.HttpStatusCode.NotFound);

            var service = new WeatherProvider(new HttpClient(mockHttp), _config);

            await Assert.ThrowsAsync<LocationNotFoundException>(() =>
                service.GetCurrentWeatherAsync("InvalidZip", "fahrenheit"));
        }

        [Fact]
        public async Task GetAverageWeatherAsync_InvalidZip_ThrowsLocationNotFoundException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*forecast*").Respond(System.Net.HttpStatusCode.NotFound);

            var service = new WeatherProvider(new HttpClient(mockHttp), _config);

            await Assert.ThrowsAsync<LocationNotFoundException>(() =>
                service.GetAverageWeatherAsync("InvalidZip", "fahrenheit", 3));
        }
    }
}

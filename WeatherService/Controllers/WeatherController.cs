using Microsoft.AspNetCore.Mvc;
using WeatherService.Services.Interfaces;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherProvider _weatherProvider;

        public WeatherController(IWeatherProvider provider) => _weatherProvider = provider;

        [HttpGet("Current/{zipCode}")]
        public async Task<IActionResult> GetCurrent(string zipCode, [FromQuery] string units)
        {
            try
            {
                var result = await _weatherProvider.GetCurrentWeatherAsync(zipCode, units);
                return Ok(new //This can be created as separate response model 'CurrentWeatherResponse' if being used at multiple places or methods.
                {
                    currentTemperature = result.Temperature,
                    unit = result.Unit,
                    lat = result.Latitude,
                    lon = result.Longitude,
                    rainPossibleToday = result.RainExpected
                });
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid zip code or parameters");
            }
            catch
            {
                return StatusCode(500, "Server error");
            }
        }

        [HttpGet("Average/{zipCode}")]
        public async Task<IActionResult> GetAverage(string zipCode, [FromQuery] string units, [FromQuery] int timePeriod)
        {
            try
            {
                var result = await _weatherProvider.GetAverageWeatherAsync(zipCode, units, timePeriod);
                return Ok(new //This can be created as separate response model 'AverageWeatherResponse, if being used at multiple places or methods.
                {
                    averageTemperature = result.Temperature,
                    unit = result.Unit,
                    lat = result.Latitude,
                    lon = result.Longitude,
                    rainPossibleInPeriod = result.RainExpected
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(500, "Server error");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WeatherService.Services.Interfaces;
using WeatherService.Models;
using WeatherService.Exceptions;

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
                //if (!(units == "C" || units =="F")) //commenting it out as it was not part of the requirement 
                //{
                //    return BadRequest("Units can either be 'C' - celcius or 'F' fahrenheit");
                //}

                var result = await _weatherProvider.GetCurrentWeatherAsync(zipCode, units);
                var response = new CurrentWeatherResponse
                {
                    CurrentTemperature = result.Temperature,
                    Unit = result.Unit,
                    Lat = result.Latitude,
                    Lon = result.Longitude,
                    RainPossibleToday = result.RainExpected
                };
                return Ok(response);

            }
            catch (LocationNotFoundException)
            {
                return BadRequest("Location not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "An Internal Server error occurred while processing your request");
            }
        }

        [HttpGet("Average/{zipCode}")]
        public async Task<IActionResult> GetAverage(string zipCode, [FromQuery] string units, [FromQuery] int timePeriod)
        {

            try
            {
                // Validate time period before calling the service 
                if (timePeriod < 2 || timePeriod > 5)
                {
                   return BadRequest("Time period must be between 2 and 5 days");
                }
                //if (!(units == "C" || units =="F")) //commenting it out as it was not part of the requirement 
                //{
                //    return BadRequest("Units can either be 'C' - celcius or 'F' fahrenheit");
                //}
                var result = await _weatherProvider.GetAverageWeatherAsync(zipCode, units, timePeriod);
                var response = new AverageWeatherResponse
                {
                    AverageTemperature = result.Temperature,
                    Unit = result.Unit,
                    Lat = result.Latitude,
                    Lon = result.Longitude,
                    RainPossibleInPeriod = result.RainExpected
                };
                return Ok(response);
            }
            catch (LocationNotFoundException)
            {
                return BadRequest("Location not found");
            }
            catch (Exception)
            {
                return StatusCode(500, "An Internal Server error occurred while processing your request");
            }
        }
    }
}

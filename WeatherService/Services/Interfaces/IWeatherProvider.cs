using WeatherService.Models;

namespace WeatherService.Services.Interfaces
{
    public interface IWeatherProvider
    {
        Task<WeatherResult> GetCurrentWeatherAsync(string zipCode, string unit);
        Task<WeatherResult> GetAverageWeatherAsync(string zipCode, string unit, int timePeriod); 
       
    }
}

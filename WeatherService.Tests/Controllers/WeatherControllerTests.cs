using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WeatherService.Controllers;
using WeatherService.Models;
using WeatherService.Services.Interfaces;
using Xunit;

namespace WeatherService.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherProvider> _mockProvider;
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        _mockProvider = new Mock<IWeatherProvider>();
        _controller = new WeatherController(_mockProvider.Object);
    }

    [Fact]
    public async Task GetCurrent_ValidZip_ReturnsOk()
    {
        _mockProvider.Setup(p => p.GetCurrentWeatherAsync("01720", "fahrenheit"))
                     .ReturnsAsync(new WeatherResult { Temperature = 22, Unit = "F", Latitude = 1, Longitude = 2, RainExpected = false });

        var result = await _controller.GetCurrent("01720", "fahrenheit");

        var okResult = Assert.IsType<OkObjectResult>(result);

        var resultValue = okResult.Value!;
        var temperatureProp = resultValue.GetType().GetProperty("currentTemperature");
        Assert.NotNull(temperatureProp);
        Assert.Equal(22, (float)temperatureProp.GetValue(resultValue)!);
    }

    [Fact]
    public async Task GetCurrent_InvalidZip_ReturnsBadRequest()
    {
        _mockProvider.Setup(p => p.GetCurrentWeatherAsync("00000", "fahrenheit"))
                     .ThrowsAsync(new ArgumentException());

        var result = await _controller.GetCurrent("00000", "fahrenheit");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetAverage_ValidInput_ReturnsOk()
    {
        _mockProvider.Setup(p => p.GetAverageWeatherAsync("01720", "fahrenheit", 3))
                     .ReturnsAsync(new WeatherResult { Temperature = 25, Unit = "F", Latitude = 1, Longitude = 2, RainExpected = true });

        var result = await _controller.GetAverage("01720", "fahrenheit", 3);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var resultValue = okResult.Value!;
        var temperatureProp = resultValue.GetType().GetProperty("averageTemperature");
        Assert.NotNull(temperatureProp);
        Assert.Equal(25, (float)temperatureProp.GetValue(resultValue)!);
    }

    [Fact]
    public async Task GetAverage_InvalidPeriod_ReturnsBadRequest()
    {
        _mockProvider.Setup(p => p.GetAverageWeatherAsync("12345", "celsius", 6))
                     .ThrowsAsync(new ArgumentException("timePeriod must be 2–5"));

        var result = await _controller.GetAverage("12345", "celsius", 6);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("timePeriod must be 2–5", bad.Value);
    }
}

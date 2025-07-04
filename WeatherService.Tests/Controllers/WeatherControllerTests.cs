using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WeatherService.Controllers;
using WeatherService.Models;
using WeatherService.Services.Interfaces;
using WeatherService.Exceptions;
using Xunit;

namespace WeatherService.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherProvider> _mockProvider;
    private readonly WeatherController _controller;
    private const string Fahrenheit = "fahrenheit";
    public WeatherControllerTests()
    {
        _mockProvider = new Mock<IWeatherProvider>();
        _controller = new WeatherController(_mockProvider.Object);
    }

    [Fact]
    public async Task GetCurrent_ValidZip_ReturnsOk()
    {
        var weather = new WeatherResult { Temperature = 22, Unit = "F", Latitude = 1, Longitude = 2, RainExpected = false };
        _mockProvider.Setup(p => p.GetCurrentWeatherAsync("01720", Fahrenheit))
                     .ReturnsAsync(weather);

        var result = await _controller.GetCurrent("01720", Fahrenheit);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<CurrentWeatherResponse>(okResult.Value);

        Assert.Equal(22, response.CurrentTemperature);
        Assert.Equal("F", response.Unit);
        Assert.Equal(1, response.Lat);
        Assert.Equal(2, response.Lon);
        Assert.False(response.RainPossibleToday);
    }

    [Fact]
    public async Task GetCurrent_InvalidZip_ReturnsBadRequest()
    {
        _mockProvider.Setup(p => p.GetCurrentWeatherAsync("00000", Fahrenheit))
                     .ThrowsAsync(new LocationNotFoundException("Location not found"));

        var result = await _controller.GetCurrent("00000", Fahrenheit);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Location not found", bad.Value);
    }

    [Fact]
    public async Task GetAverage_ValidInput_ReturnsOk()
    {
        var weather = new WeatherResult { Temperature = 25, Unit = "F", Latitude = 1, Longitude = 2, RainExpected = true };
        _mockProvider.Setup(p => p.GetAverageWeatherAsync("01720", Fahrenheit, 3))
                     .ReturnsAsync(weather);

        var result = await _controller.GetAverage("01720", Fahrenheit, 3);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AverageWeatherResponse>(okResult.Value);

        Assert.Equal(25, response.AverageTemperature);
        Assert.Equal("F", response.Unit);
        Assert.Equal(1, response.Lat);
        Assert.Equal(2, response.Lon);
        Assert.True(response.RainPossibleInPeriod);
    }

    [Fact]
    public async Task GetAverage_InvalidPeriod_ReturnsBadRequest()
    {
        // Controller itself handles this validation, no need for service to throw
        var result = await _controller.GetAverage("12345", "celsius", 6);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Time period must be between 2 and 5 days", bad.Value);
    }

    [Fact]
    public async Task GetAverage_LocationNotFound_ReturnsBadRequest()
    {
        _mockProvider.Setup(p => p.GetAverageWeatherAsync("00000", Fahrenheit, 3))
                     .ThrowsAsync(new LocationNotFoundException("Location not found"));

        var result = await _controller.GetAverage("00000", Fahrenheit, 3);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Location not found", bad.Value);
    }

    [Fact]
    public async Task GetCurrent_UnexpectedException_Returns500()
    {
        _mockProvider.Setup(p => p.GetCurrentWeatherAsync("01720", Fahrenheit))
                     .ThrowsAsync(new Exception("Unexpected"));

        var result = await _controller.GetCurrent("01720", Fahrenheit);

        var error = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, error.StatusCode);
    }
}
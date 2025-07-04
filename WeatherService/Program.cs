using WeatherService.Services;
using WeatherService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IWeatherProvider, WeatherProvider>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();
app.Run();
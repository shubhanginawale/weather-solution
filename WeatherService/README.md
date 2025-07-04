# WeatherService API

This is a .NET 8 Web API that fetches weather data from OpenWeatherMap and exposes two endpoints:

- `/weather/current/{zip}` — get current weather
- `/weather/average/{zip}` — get average weather over a given time period (2–5 days)

## Authentication
All endpoints are protected via **API Key** authentication.  
Clients must include a header in every request:

```
X-API-KEY: your-api-key
```

You can configure the API key in `appsettings.json` or via environment variables.

---

## Running the API

### With Docker

```bash
docker build -t weatherservice -f WeatherService/Dockerfile .

docker run -p 5000:5000 \
  -e AppSettings__ApiKey=your-internal-api-key \
  -e OpenWeatherMap__ApiKey=your-openweathermap-api-key \
  weatherservice
```

The API will be available at:  
[http://localhost:5000/weather](http://localhost:5000/weather)---

### 🧪 Without Docker (local dev)

```bash
cd WeatherService
dotnet build
dotnet run
```

Configure your `appsettings.json`:

```json
{
  "AppSettings": {
    "ApiKey": "DCU-WeatheAPIKey-2025"
  },
  "OpenWeatherMap": {
    "ApiKey": "your-openweathermap-api-key"
  }
}
```

---

## Example Requests

### Get current weather

```
GET /weather/current/10001?units=celsius
X-API-KEY: DCU-WeatheAPIKey-2025
```

### Get average weather (past 2 days)

```
GET /weather/average/10001?units=fahrenheit&timePeriod=2
X-API-KEY: DCU-WeatheAPIKey-2025
```

---

## Testing

Unit tests use `xUnit` and `Moq`.

Run them with:

```bash
cd WeatherService.Tests
dotnet test
```

- Controller tests mock `IWeatherProvider`
- Provider tests mock `HttpClient` using `RichardSzalay.MockHttp`

---

## Project Structure

```
WeatherService/
├── Controllers/
├── Middleware/           # API key validation
├── Models/
├── Services/             # OpenWeatherMap API logic
├── appsettings.json
├── Program.cs
```

---

## Dependencies

- .NET 8
- OpenWeatherMap API
- xUnit, Moq, MockHttp (for tests)

---

## Environment Variables

| Key                        | Description                      |
|----------------------------|----------------------------------|
| `AppSettings__ApiKey`      | API key to secure your endpoints |
| `OpenWeatherMap__ApiKey`   | Your OpenWeatherMap API key      |

---

## Example CLI Client

Use `weatherCLI` to call this API interactively with YAML/JSON/text output and login-based key storage.

See `weatherCLI/README.md` for usage.
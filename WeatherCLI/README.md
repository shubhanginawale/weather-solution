# Weather CLI

A .NET 8 console application that consumes the WeatherService API and displays current or average weather in multiple formats.

##  Commands

### Get current weather
```bash
dotnet run --project ./weathercli get-current-weather <zip> <units> [--output text|json|yaml] [--apikey <key>]
```

### Get average weather
```bash
dotnet run --project ./weathercli get-average-weather <zip> <units> <days> [--output text|json|yaml] [--apikey <key>]
```

### Login (store API key)
```bash
dotnet run --project weathercli -- login <apikey>
```
##  Authentication
This CLI requires an API key for the WeatherService API. Either:
- Pass it via `--apikey`, or
- Store it using `dotnet run --project weathercli -- login <key>`

##  Output Formats
- `text` – human-friendly summary (default)
- `json` – raw JSON output
- `yaml` – YAML formatted output

##  Build & Run
```bash
# Build
cd weatherCLI
 dotnet build

# Run (after login)
dotnet run --project ./weathercli -- get-average-weather 10001 celsius --output yaml --apikey yourapikey
```

Or directly:
```bash
dotnet run --project ./weathercli -- get-average-weather 10001 fahrenheit 3 --output yaml --apikey yourapikey
```

## Example
```bash
dotnet run --project weathercli -- login DCU-WeatherAPIKey-2025
dotnet run --project ./weathercli -- get-current-weather 01720 fahrenheit --output json --apikey DCU-WeatherAPIKey-2025
dotnet run --project ./weathercli -- get-average-weather 01720 fahrenheit 4 --output json --apikey DCU-WeatherAPIKey-2025
```

## Project Structure
```
weatherCLI/
├── Commands/               # System.CommandLine commands
├── Models/                 # WeatherResult shared with API
├── Helpers/                # Output formatting + login support
├── Services/               # API client for WeatherService
├── Program.cs              # Entry point
```

## Environment
- .NET 8
- Requires access to running WeatherService API (default: http://localhost:5000)

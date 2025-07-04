# Weather CLI

A .NET 8 console application that consumes the WeatherService API and displays current or average weather in multiple formats.

##  Commands

### Get current weather
```bash
weatherCLI get-current-weather <zip> <units> [--output text|json|yaml] [--apikey <key>]
```

### Get average weather
```bash
weatherCLI get-average-weather <zip> <units> <days> [--output text|json|yaml] [--apikey <key>]
```

### Login (store API key)
```bash
weatherCLI login <apikey>
```
Once stored, you don’t need to pass `--apikey` again.


##  Authentication
This CLI requires an API key for the WeatherService API. Either:
- Pass it via `--apikey`, or
- Store it using `weatherCLI login <key>`

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
dotnet run -- get-current-weather 10001 celsius
```

Or directly:
```bash
dotnet run -- get-average-weather 10001 fahrenheit 3 --output yaml
```

## Example
```bash
weatherCLI login your-api-key
weatherCLI get-current-weather 90210 celsius --output text
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

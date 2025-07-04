namespace WeatherService.Models
{
    public class AverageWeatherResponse
    {
        public float AverageTemperature { get; set; }
        public string Unit { get; set; } = "F";
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool RainPossibleInPeriod { get; set; }
    }
}

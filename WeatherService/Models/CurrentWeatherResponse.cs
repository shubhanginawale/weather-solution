namespace WeatherService.Models
{
    public class CurrentWeatherResponse
    {
        public float CurrentTemperature { get; set; }
        public string Unit { get; set; } = "F";
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool RainPossibleToday { get; set; }
    }
}

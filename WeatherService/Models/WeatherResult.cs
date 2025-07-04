namespace WeatherService.Models
{
    public class WeatherResult
    {
        public float Temperature { get; set; }
        public string Unit { get; set; } = "C";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool RainExpected { get; set; }
    }
}

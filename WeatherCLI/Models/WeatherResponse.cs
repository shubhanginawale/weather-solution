namespace WeatherCLI.Models
{
    public class WeatherResponse
    {
       
        public string Unit { get; set; } = "C";
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}

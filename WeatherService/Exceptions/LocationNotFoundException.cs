namespace WeatherService.Exceptions
{
    public class LocationNotFoundException : Exception
    {
        public LocationNotFoundException(string message) : base(message) { }
    }
}

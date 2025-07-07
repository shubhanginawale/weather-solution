using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCLI.Models
{
    public class AverageWeatherResponse : WeatherResponse
    {
        public float AverageTemperature { get; set; }
        public bool RainPossibleInPeriod { get; set; }
    }
}

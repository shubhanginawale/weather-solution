using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCLI.Models
{
    public class CurrentWeatherResponse : WeatherResponse
    {
        public float CurrentTemperature { get; set; }
        public bool RainPossibleToday { get; set; }
    }
}

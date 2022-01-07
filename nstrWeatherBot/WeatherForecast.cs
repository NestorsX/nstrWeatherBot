using System.Collections.Generic;

namespace nstrWeatherBot
{
    public class WeatherForecast
    {
        public List<DailyForecasts> DailyForecasts { get; set; }
    }

    public class DailyForecasts
    {
        public Temperature Temperature { get; set; }
        public Day Day { get; set; }
        public Night Night { get; set; }
    }

    public class Temperature
    {
        public Minimum Minimum { get; set; }
        public Minimum Maximum { get; set; }
    }

    public class Minimum
    {
        public double Value { get; set; }
    }

    public class Maximum
    {
        public double Value { get; set; }
    }

    public class Day
    {
        public string IconPhrase { get; set; }
    }
    public class Night
    {
        public string IconPhrase { get; set; }
    }
}

namespace nstrWeatherBot
{
    public class CityInfo
    {
        public string Key { get; set; }
        public string LocalizedName { get; set; }
        public Country Country { get; set; }
    }

    public class Country
    {
        public string LocalizedName { get; set; }
    }
}

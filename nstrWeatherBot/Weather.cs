using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;

namespace nstrWeatherBot
{
    public class Weather
    {
        private const string _apiKey = "UNuD0g26dPS5PLyDNOA6oLJuMX4RBYxR";
        private readonly Dictionary<string, string> _weatherStates = new()
        {
            { "Солнечно", "🔅" },
            { "Ясно", "🔅" },
            { "Преимущественно солнечно", "🌤" },
            { "Облачно с прояснениями", "⛅" },
            { "Переменная облачность", "⛅" },
            { "Преимущественно облачно", "☁" },
            { "Облачно", "☁" },
            { "Небольшая облачность с дождями", "🌤🌧" },
            { "Переменная облачность с дождем", "⛅🌧" },
            { "Преимущественно облачно с ливнями", "☁🌧" },
            { "Ливни", "🌧" },
            { "Преимущественно ясно", "🌤" },
            { "Преимущественно облачно, небольшой снег", "☁🌨" },
            { "Небольшой снег", "🌨" },
            { "Снег", "🌨" },
        };

        public string GetWeatherInfo(string CityName)
        {
            var webClient = new WebClient();
            try
            {
                // Receive information about requested city
                string cityJSON = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={_apiKey}&q={CityName}&language=ru-ru";
                string cityJSON_string = webClient.DownloadString(cityJSON);
                ObservableCollection<CityInfo> cityInfo = JsonSerializer.Deserialize<ObservableCollection<CityInfo>>(cityJSON_string);
                string cityInfoPattern = "📍 {0}, {1}\n\n";
                // Receive information about weather in the requested city
                string weatherForecastJSON = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{cityInfo[0].Key}?apikey={_apiKey}&language=ru-ru&metric=true";
                string weatherForecastJSON_string = webClient.DownloadString(weatherForecastJSON);
                WeatherForecast weatherforecast = JsonSerializer.Deserialize<WeatherForecast>(weatherForecastJSON_string);
                string weatherForecastPattern = "☀️ Днем:\n {0} °C, {1} " + _weatherStates[weatherforecast.DailyForecasts[0].Day.IconPhrase] + "\n\n" + "🌙 Ночью: \n {2} °C, {3} " + _weatherStates[weatherforecast.DailyForecasts[0].Night.IconPhrase] + "\n\nДанные предоставлены ☀️AccuWeather";
                return string.Format(cityInfoPattern, cityInfo[0].LocalizedName, cityInfo[0].Country.LocalizedName)
                    + string.Format(weatherForecastPattern, weatherforecast.DailyForecasts[0].Temperature.Maximum.Value, weatherforecast.DailyForecasts[0].Day.IconPhrase,
                    weatherforecast.DailyForecasts[0].Temperature.Minimum.Value, weatherforecast.DailyForecasts[0].Night.IconPhrase);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n!!!---WARNING---!!!\nError:\n" + ex.Message + "\nStack trace:\n" + ex.StackTrace + "\n============\n");
                return "Не удалось отобразить погоду в запрашиваемой точке.\nОбратитесь к администратору: @NestorsX";
            }
        }
    }
}

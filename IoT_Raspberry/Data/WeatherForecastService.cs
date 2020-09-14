using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IoT_Api.Helpers;

namespace IoT_RaspberryServer.Data
{
    public class WeatherForecastService
    {
        // TODO: add possibility of configuration(.json?)
        private const string ApiUri = "https://api.openweathermap.org/data/2.5/onecall?lat=54.352051&lon=18.64637&lang=pl&units=metric&exclude=minutely&appid=";

        private HttpClient httpClient = new HttpClient();

        public List<WeatherForecast> GetForecast()
        {
            List<WeatherForecast> forecastList = new List<WeatherForecast>();
            string json = this.GetWeatherJsonData().Result;

            dynamic deserializedJson = JsonConvert.DeserializeObject(json);

            foreach (var hourForecast in deserializedJson.hourly)
            {
                forecastList.Add(new WeatherForecast
                {
                    Date = DateTimeHelpers.ConvertFromUnixTime((double)hourForecast.dt.Value),
                    FeelsLikeTemp = (double)hourForecast.feels_like.Value,
                    Humidity = (double)hourForecast.humidity.Value,
                    Pressure = (int)hourForecast.pressure.Value,
                    Temp = (double)hourForecast.temp.Value,
                    Weather = hourForecast.weather[0].description.Value,
                    IconAddress = this.GetWeatherIconAddress(hourForecast.weather[0].icon.Value),
                    WeatherId = (int)hourForecast.weather[0].id.Value
                });
            }

            return forecastList;
        }

        public List<WeatherForecast> Forecasts = new List<WeatherForecast>();

        public string LastUpdateTime { get; private set; }

        //todo: there's no need to download new forecast every time if they're already up to date.
        //example would be if someone clicks between hourly/daily the same hour or if someone loads the page again in the same hour.

        /*
         * 
         * {
            "dt": 1597939200,
            "temp": 22.63,
            "feels_like": 21.82,
            "pressure": 1013,
            "humidity": 60,
            "dew_point": 14.48,
            "clouds": 100,
            "visibility": 10000,
            "wind_speed": 3.19,
            "wind_deg": 21,
            "weather": [
                {
                    "id": 804,
                    "main": "Clouds",
                    "description": "całkowite zachmurzenie",
                    "icon": "04d"
                }
            ],
            "pop": 0
        },
        */

        public List<WeatherForecast> GetHourlyForecast()
        {
            List<WeatherForecast> forecastList = new List<WeatherForecast>();
            string json = this.GetWeatherJsonData().Result;

            dynamic deserializedJson = JsonConvert.DeserializeObject(json);

            foreach (var hourForecast in deserializedJson.hourly)
            {
                forecastList.Add(new WeatherForecast
                {
                    Date = DateTimeHelpers.ConvertFromUnixTime((double)hourForecast.dt.Value),
                    FeelsLikeTemp = (double)hourForecast.feels_like.Value,
                    Humidity = (double)hourForecast.humidity.Value,
                    Pressure = (int)hourForecast.pressure.Value,
                    Temp = (double)hourForecast.temp.Value,
                    Weather = hourForecast.weather[0].description.Value,
                    IconAddress = this.GetWeatherIconAddress(hourForecast.weather[0].icon.Value),
                    WeatherId = (int)hourForecast.weather[0].id.Value
                });
            }

            return forecastList;
        }



        /*
         * {
            "dt": 1597917600,
            "sunrise": 1597894313,
            "sunset": 1597946754,
            "temp": {
                "day": 22.63,
                "min": 19.69,
                "max": 22.63,
                "night": 19.69,
                "eve": 22.63,
                "morn": 22.63
            },
            "feels_like": {
                "day": 21.73,
                "night": 19.71,
                "eve": 21.73,
                "morn": 21.73
            },
            "pressure": 1013,
            "humidity": 60,
            "dew_point": 14.48,
            "wind_speed": 3.32,
            "wind_deg": 25,
            "weather": [
                {
                    "id": 804,
                    "main": "Clouds",
                    "description": "całkowite zachmurzenie",
                    "icon": "04d"
                }
            ],
            "clouds": 100,
            "pop": 0,
            "uvi": 5.34
        },
        */

        public List<WeatherForecast> GetDailyForecast()
        {
            List<WeatherForecast> forecastList = new List<WeatherForecast>();
            string json = this.GetWeatherJsonData().Result;

            dynamic deserializedJson = JsonConvert.DeserializeObject(json);

            foreach (var dailyForecast in deserializedJson.daily)
            {
                forecastList.Add(new WeatherForecast
                {
                    Date = DateTimeHelpers.ConvertFromUnixTime((double)dailyForecast.dt.Value),
                    FeelsLikeTemp = (double)dailyForecast.feels_like.day.Value,
                    Humidity = (double)dailyForecast.humidity.Value,
                    Pressure = (int)dailyForecast.pressure.Value,
                    Temp = (double)dailyForecast.temp.day.Value,
                    Weather = dailyForecast.weather[0].description.Value,
                    IconAddress = this.GetWeatherIconAddress(dailyForecast.weather[0].icon.Value),
                    WeatherId = (int)dailyForecast.weather[0].id.Value
                });
            }

            return forecastList;
        }

        private Task<string> GetWeatherJsonData()
        {
            var response = httpClient.GetAsync(ApiUri + AppSettings.OpenWeatherApiKey);

            switch (response.Result.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedApiAccessException("Invalid API key.");
                case HttpStatusCode.NotFound:
                    throw new LocationNotFoundException("Location not found.");
                case HttpStatusCode.OK:
                    return response.Result.Content.ReadAsStringAsync();
                default:
                    throw new NotImplementedException(response.Result.StatusCode.ToString());
            }
        }

        private string GetWeatherIconAddress(string iconId)
        {
            return $"http://openweathermap.org/img/wn/{iconId}.png";
        }
    }

    internal class LocationNotFoundException : Exception
    {
        public LocationNotFoundException(string? message) : base(message)
        {
        }
    }

    internal class UnauthorizedApiAccessException : Exception
    {
        public UnauthorizedApiAccessException(string? message) : base(message)
        {
        }
    }
}

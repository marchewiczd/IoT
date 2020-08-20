using System;

namespace IoT_Raspberry.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public string IconAddress { get; set; }

        public double Temp { get; set; }

        public double FeelsLikeTemp { get; set; }

        public double Humidity { get; set; }

        public int Pressure { get; set; }

        public int WeatherId { get; set; }

        public string Weather { get; set; }
    }
}

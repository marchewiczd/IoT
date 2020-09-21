using System.Collections.Generic;
using System.Globalization;

namespace IoT_RaspberryServer.Data
{
    //TODO: make it configurable
    public static class AppSettings
    {
        public const string LiteDbFilePath = @".\sprinklers.db";

        public static string OpenWeatherApiKey { get; set; }


        public static List<CultureInfo> SupportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("pl")
        };
    }
}

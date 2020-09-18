namespace IoT_RaspberryServer.Data
{
    //TODO: make it configurable
    public static class AppSettings
    {
        public const string LiteDbFilePath = @".\sprinklers.db";

        public static string OpenWeatherApiKey { get; set; }

        
    }
}

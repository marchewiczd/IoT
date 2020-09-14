using System;

namespace IoT_Api.Helpers
{
    public static class DateTimeHelpers
    {
        public static DateTime ConvertFromUnixTime(double unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)unixTime).ToLocalTime();
        }

    }
}

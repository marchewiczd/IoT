using System;
using System.Runtime.CompilerServices;

namespace IoT_Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ConvertToUnixTime(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();
        }
    }
}

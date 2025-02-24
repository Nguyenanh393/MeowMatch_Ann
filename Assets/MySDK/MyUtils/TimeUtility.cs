using System;

namespace MyUtils
{
    public static class TimeUtility
    {
        public static string GetCurrentTime()
        {
            return DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}


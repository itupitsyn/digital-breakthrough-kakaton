namespace Hencoder.Services
{
    public static class Timestamp
    {
        public static long UtcNow => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public static long UtcNowAddDays(int days) => DateTimeOffset.UtcNow.AddDays(days).ToUnixTimeMilliseconds();
        public static long AddDays(long utcDate, int days) => DateTimeOffset.FromUnixTimeMilliseconds(utcDate).AddDays(days).ToUnixTimeMilliseconds();
        public static long UtcNowAddSeconds(int seconds) => DateTimeOffset.UtcNow.AddSeconds(seconds).ToUnixTimeMilliseconds();
        public static long UtcNowAddTimeSpan(TimeSpan period) => DateTimeOffset.UtcNow.Add(period).ToUnixTimeMilliseconds();
        public static long Max => DateTimeOffset.MaxValue.ToUnixTimeMilliseconds();
        public static long Min => DateTimeOffset.MinValue.ToUnixTimeMilliseconds();
        public static DateTime ToLocalTime(long timeStamp) => DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).LocalDateTime;
    }
}

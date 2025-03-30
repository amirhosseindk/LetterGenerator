using System.Globalization;

namespace LetterGenerator.Shared.Extentions
{
    public static class PersianDateExtension
    {
        public static DateTime ToIranLocalTime(this DateTime utc)
        {
            var iranZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, iranZone);
        }

        public static string ToPersianString(this DateTime dt)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(dt)}/{pc.GetMonth(dt):00}/{pc.GetDayOfMonth(dt):00}";
        }
    }
}
using System.Globalization;

namespace LetterGenerator.Shared.Extentions
{
    public static class PersianDateExtension
    {
        public static string ToPersian(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime):00}/{pc.GetDayOfMonth(dateTime):00}";
        }
    }
}
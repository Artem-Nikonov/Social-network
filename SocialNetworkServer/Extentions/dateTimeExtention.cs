using System.Runtime.CompilerServices;

namespace SocialNetworkServer.Extentions
{
    public static class DateTimeExtention
    {
        public static DateTime RemoveSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
        public static string GetSpecialFormat(this DateTime dateTime)
        {
            var dateTimeString = $"{dateTime.Day}.{dateTime.Month}.{dateTime.Year} {dateTime.Hour}:{dateTime.Minute}";
            return dateTimeString;
        }
    }
}

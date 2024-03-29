using System.Runtime.CompilerServices;

namespace SocialNetworkServer.Extentions
{
    public static class dateTimeExtention
    {
        public static DateTime RemoveSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }
    }
}

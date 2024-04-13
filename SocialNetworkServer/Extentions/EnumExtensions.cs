using SocialNetworkServer.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SocialNetworkServer.Extentions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? enumValue.ToString();
        }
    }
}

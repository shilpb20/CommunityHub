using System.Reflection;
using System.Runtime.Serialization;

namespace CommunityHub.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue(this Enum value) =>
            value.GetType()
                 .GetField(value.ToString())
                 .GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
    }
}
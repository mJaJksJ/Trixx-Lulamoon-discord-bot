using System.Reflection;

namespace Trixx.Common.Utils
{
    public static class EnumUtils
    {
        public static T TryGetCustomAttribute<T>(this Enum value)
            where T : Attribute
        {
            var stringValue = value.ToString();

            return value
                .GetType()
                .GetMember(stringValue)
                .FirstOrDefault()
                ?.GetCustomAttribute<T>()
                ?? throw new ArgumentException("Can't cast attribute");
        }
    }
}

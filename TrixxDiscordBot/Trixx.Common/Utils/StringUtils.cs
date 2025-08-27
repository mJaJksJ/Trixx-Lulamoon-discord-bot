using System.Text.RegularExpressions;

namespace Trixx.Common.Utils
{
    public static partial class StringUtils
    {
        public static Regex NormalizeRegex => _normalizeRegex();

        [GeneratedRegex("[^a-zA-Zа-яА-Я0-9]")]
        private static partial Regex _normalizeRegex();

        public static string Normalize(this string str)
        {
            return NormalizeRegex.Replace(str.ToUpper(), string.Empty);
        }
    }
}

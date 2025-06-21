namespace Trixx.Common.Utils
{
    public static class DatetimeUtils
    {
        public static string As_ddMMyyyy(this DateTimeOffset dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy");
        }

        public static string As_ddMMyyyy(this DateTimeOffset? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.Value.As_ddMMyyyy()
                : string.Empty;
        }
    }
}

namespace Firefly.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool TryParseNullOrEmpty(this string value, out string result)
        {
            result = value ?? string.Empty;

            return result.IsNullOrEmpty();
        }

        public static bool IsNullOrWhitespace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}

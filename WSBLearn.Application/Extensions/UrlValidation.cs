namespace WSBLearn.Application.Extensions
{
    public static class UrlValidation
    {
        public static bool UrlOrEmpty(this string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return url is string valueAsString &&
                   (valueAsString.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || valueAsString.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                    || valueAsString.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase));
        }
    }
}

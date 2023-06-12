namespace LearningApp.Application.Extensions
{
    public static class UrlValidation
    {
        public static bool UrlOrEmpty(this string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            string[] validSchemes = { "http://", "https://", "ftp://" };
            return validSchemes.Any(scheme => url.StartsWith(scheme, StringComparison.OrdinalIgnoreCase));
        }
    }
}

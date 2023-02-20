namespace LearningApp.Application.Settings
{
    public class JwtAuthenticationSettings
    {
        public string Key { get; set; }
        public int ExpireDays { get; set; }
        public string Issuer { get; set; }
    }
}

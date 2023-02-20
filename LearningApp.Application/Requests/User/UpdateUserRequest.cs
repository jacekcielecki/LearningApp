namespace LearningApp.Application.Requests.User
{
    public class UpdateUserRequest
    {
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}

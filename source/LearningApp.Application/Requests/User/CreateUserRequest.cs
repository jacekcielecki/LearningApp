namespace LearningApp.Application.Requests.User
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}

namespace LearningApp.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public RoleDto Role { get; set; }
        public UserProgressDto UserProgress { get; set; }
    }
}

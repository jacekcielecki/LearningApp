namespace LearningApp.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsVerified { get; set; }
        public virtual RoleDto Role { get; set; }
        public virtual UserProgressDto UserProgress { get; set; }
    }
}

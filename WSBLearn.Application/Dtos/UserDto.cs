using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public virtual Role Role { get; set; }
    }
}

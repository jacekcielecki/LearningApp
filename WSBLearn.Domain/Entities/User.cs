using WSBLearn.Domain.Constants;

namespace WSBLearn.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Username { get; set; }
        public Role? Role { get; set; }
    }
}

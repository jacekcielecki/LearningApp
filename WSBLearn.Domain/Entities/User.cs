namespace WSBLearn.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}

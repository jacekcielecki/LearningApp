namespace LearningApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string VerificationToken { get; set; }
        public DateTime VerificationTokenExpireDate { get; set; }
        public bool IsVerified { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int UserProgressId { get; set; }
        public virtual UserProgress UserProgress { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}

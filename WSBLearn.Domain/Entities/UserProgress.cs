namespace WSBLearn.Domain.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public int TotalCompletedQuiz { get; set; }
        public int TotalCompletedCategory { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CategoryProgress> CategoryProgress { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
    }
}

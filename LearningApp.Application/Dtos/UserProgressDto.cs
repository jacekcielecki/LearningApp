namespace LearningApp.Application.Dtos
{
    public class UserProgressDto
    {
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public int TotalCompletedQuiz { get; set; }
        public int TotalCompletedCategory { get; set; }
        public virtual ICollection<CategoryProgressDto> CategoryProgress { get; set; }
        public virtual ICollection<AchievementDto> Achievements { get; set; }
    }
}

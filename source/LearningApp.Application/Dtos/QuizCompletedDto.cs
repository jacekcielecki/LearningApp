namespace LearningApp.Application.Dtos
{
    public class QuizCompletedDto
    {
        public int TotalExperiencePoints { get; set; }
        public int TotalCompletedQuiz { get; set; }
        public int CurrentUserLevel { get; set; }
        public bool IsCategoryLevelCompleted { get; set; }
    }
}

namespace WSBLearn.Application.Responses
{
    public class QuizCompletedResponse
    {
        public int TotalExperiencePoints { get; set; }
        public int TotalCompletedQuiz { get; set; }
        public int CurrentUserLevel { get; set; }
        public bool IsCategoryLevelCompleted { get; set; }
    }
}

namespace LearningApp.Application.Requests.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int QuestionsPerQuiz { get; set; }
        public int QuizPerLevel { get; set; }
    }
}

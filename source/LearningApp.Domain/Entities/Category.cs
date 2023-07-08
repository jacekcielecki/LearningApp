namespace LearningApp.Domain.Entities
{
    public class Category : UserContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int QuestionsPerQuiz { get; set; }
        public int QuizPerLevel { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<CategoryGroup> CategoryGroups { get; set; }
    }
}

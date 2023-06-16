namespace LearningApp.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int QuestionsPerQuiz { get; set; }
        public int QuizPerLevel { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<CategoryGroup> CategoryGroups { get; set; }
    }
}

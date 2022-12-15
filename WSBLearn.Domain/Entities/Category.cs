namespace WSBLearn.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int QuestionsPerLesson { get; set; }
        public int LessonsPerLevel { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<CategoryGroup> CategoryGroups { get; set; }
    }
}

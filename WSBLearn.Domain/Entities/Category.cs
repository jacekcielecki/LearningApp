namespace WSBLearn.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int QuestionsPerLesson { get; set; }
        public int LessonsPerLevel { get; set; }
    }
}

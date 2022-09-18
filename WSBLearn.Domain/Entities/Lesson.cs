namespace WSBLearn.Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string? LessonName { get; set; }
        public int TotalLevels { get; set; }
        public int QuestionsPerLevel { get; set; }
        public IQueryable<Question>? Questions { get; set; }
    }
}

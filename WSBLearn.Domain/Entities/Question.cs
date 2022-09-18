namespace WSBLearn.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int LessonLevel { get; set; }
        public string? QuestionContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public string? CorrectAnswer { get; set; }

        public int LessonId { get; set; }
    }
}

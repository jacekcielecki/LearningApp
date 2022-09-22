namespace WSBLearn.Domain.Entities
{
    public class Question : BaseEntity
    {
        public int QuestionLevel { get; set; }
        public string QuestionContent { get; set; }
        public string? ImageUrl { get; set; }
        public KeyValuePair<char, string>? Answers { get; set; }
        public char CorrectAnswer { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

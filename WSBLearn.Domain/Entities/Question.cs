using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public char CorrectAnswer { get; set; }
        public int Level { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}

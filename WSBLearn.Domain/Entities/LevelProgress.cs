using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class LevelProgress
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public int FinishedQuizzes { get; set; }
        public int QuizzesToFinish { get; set; }
        public bool LevelCompleted { get; set; }

        [Required]
        public int CategoryProgressId { get; set; }
        [JsonIgnore]
        public CategoryProgress CategoryProgress { get; set; }
    }
}

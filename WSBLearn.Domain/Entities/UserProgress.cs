using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class UserProgress
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public int TotalCompletedQuiz { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        public virtual ICollection<CategoryProgress> CategoryProgress { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
    }
}

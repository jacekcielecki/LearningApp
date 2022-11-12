using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class UserProgress
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}

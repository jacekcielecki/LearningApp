using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserProgress> UserProgresses { get; set; }
    }
}

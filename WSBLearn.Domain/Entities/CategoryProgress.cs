using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class CategoryProgress
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }

        [Required]
        public int UserProgressId { get; set; }
        [JsonIgnore]
        public UserProgress UserProgress { get; set; }
    }
}

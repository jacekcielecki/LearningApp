using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WSBLearn.Domain.Entities
{
    public class CategoryProgress
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public int UserProgressId { get; set; }
        [JsonIgnore]
        public UserProgress UserProgress { get; set; }
    }
}

namespace LearningApp.Domain.Entities
{
    public class CategoryGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? IconUrl { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}

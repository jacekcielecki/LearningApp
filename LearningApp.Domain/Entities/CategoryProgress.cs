namespace LearningApp.Domain.Entities
{
    public class CategoryProgress
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public bool CategoryCompleted { get; set; }
        public int UserProgressId { get; set; }
        public UserProgress UserProgress { get; set; }
        public virtual ICollection<LevelProgress> LevelProgresses { get; set; }
    }
}

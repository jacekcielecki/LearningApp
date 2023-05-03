namespace LearningApp.Application.Dtos
{
    public class CategoryProgressDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public bool CategoryCompleted { get; set; }
        public virtual ICollection<LevelProgressDto> LevelProgresses { get; set; }
    }
}

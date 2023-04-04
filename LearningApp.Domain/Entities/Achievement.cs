namespace LearningApp.Domain.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserProgress> UserProgresses { get; set; }
    }
}

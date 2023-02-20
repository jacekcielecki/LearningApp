namespace LearningApp.Domain.Entities
{
    public class LevelProgress
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public int FinishedQuizzes { get; set; }
        public int QuizzesToFinish { get; set; }
        public bool LevelCompleted { get; set; }
        public int CategoryProgressId { get; set; }
        public CategoryProgress CategoryProgress { get; set; }
    }
}

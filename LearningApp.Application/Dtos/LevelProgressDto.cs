namespace LearningApp.Application.Dtos
{
    public class LevelProgressDto
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public int FinishedQuizzes { get; set; }
        public int QuizzesToFinish { get; set; }
        public bool LevelCompleted { get; set; }
    }
}

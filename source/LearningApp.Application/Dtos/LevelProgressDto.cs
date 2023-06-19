namespace LearningApp.Application.Dtos
{
    public class LevelProgressDto
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public int FinishedQuiz { get; set; }
        public int QuizToFinish { get; set; }
        public bool LevelCompleted { get; set; }
    }
}

﻿namespace LearningApp.Application.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int QuestionsPerQuiz { get; set; }
        public int QuizPerLevel { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public virtual UserDto Creator { get; set; }
        public virtual ICollection<QuestionDto> Questions { get; set; }
    }
}

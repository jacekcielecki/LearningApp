﻿namespace LearningApp.Application.Requests.Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int QuestionsPerLesson { get; set; }
        public int LessonsPerLevel { get; set; }
    }
}

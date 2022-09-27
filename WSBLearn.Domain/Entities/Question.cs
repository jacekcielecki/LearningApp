﻿using System.Collections.ObjectModel;

namespace WSBLearn.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string QuestionContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public char CorrectAnswer { get; set; }
        public int Level { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}

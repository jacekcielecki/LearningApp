namespace WSBLearn.Application.Dtos
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int QuestionsPerLevel { get; set; }
        public int Levels { get; set; }
    }
}

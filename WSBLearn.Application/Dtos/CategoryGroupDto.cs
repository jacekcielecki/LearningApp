namespace WSBLearn.Application.Dtos
{
    public class CategoryGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public virtual ICollection<CategoryDto> Categories { get; set; }
    }
}

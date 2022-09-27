using WSBLearn.Application.Dtos;

namespace WSBLearn.Application.Interfaces
{
    public interface ICategoryService
    {
        int? Create(CategoryDto categoryDto);
        IEnumerable<CategoryDto>? GetAll();
        CategoryDto GetById(int id);
        CategoryDto Update(int id, CategoryDto categoryDto);
        void Delete(int id);
    }
}

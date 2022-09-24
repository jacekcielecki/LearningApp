using WSBLearn.Application.Dtos;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Services
{
    public interface ICategoryService
    {
        int? Create(CategoryDto categoryDto);
        IEnumerable<CategoryDto>? GetAll();
        CategoryDto GetById(int id);
        CategoryDto? Update(int id, CategoryDto categoryDto);
        void Delete(int id);
    }
}

using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests;

namespace WSBLearn.Application.Interfaces
{
    public interface ICategoryService
    {
        int? Create(CreateCategoryRequest createCategoryRequest);
        IEnumerable<CategoryDto>? GetAll();
        CategoryDto GetById(int id);
        CategoryDto Update(int id, UpdateCategoryRequest updateCategoryRequest);
        void Delete(int id);
    }
}

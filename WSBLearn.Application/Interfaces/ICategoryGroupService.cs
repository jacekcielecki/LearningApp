using WSBLearn.Application.Dtos;
using WSBLearn.Application.Requests.CategoryGroup;

namespace WSBLearn.Application.Interfaces
{
    public interface ICategoryGroupService
    {
        int Create(CreateCategoryGroupRequest request);
        IEnumerable<CategoryGroupDto> GetAll();
        CategoryGroupDto GetById(int id);
        CategoryGroupDto Update(int id, UpdateCategoryGroupRequest request);
        void Delete(int id);
        CategoryGroupDto AddCategory(int id, int categoryId);
        CategoryGroupDto RemoveCategory(int id, int categoryId);
    }
}

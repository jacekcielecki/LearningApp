using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.CategoryGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryGroupController : ControllerBase
    {
        private readonly ICategoryGroupService _categoryGroupService;

        public CategoryGroupController(ICategoryGroupService categoryGroupService)
        {
            _categoryGroupService = categoryGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _categoryGroupService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _categoryGroupService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryGroupRequest request)
        {
            var response = await _categoryGroupService.CreateAsync(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryGroupRequest request)
        {
            var response = await _categoryGroupService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpPut("addCategory/{id:int}/{categoryId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(int id, int categoryId)
        {
            var response = await _categoryGroupService.AddCategoryAsync(id, categoryId);
            return Ok(response);
        }

        [HttpPut("removeCategory/{id:int}/{categoryId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveCategory(int id, int categoryId)
        {
            var response = await _categoryGroupService.RemoveCategoryAsync(id, categoryId);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _categoryGroupService.DeleteAsync(id);
            return Ok();
        }
    }
}

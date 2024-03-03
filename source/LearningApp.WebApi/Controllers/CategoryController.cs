using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Category;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var userContext = HttpContext.GetUserContext();
            var response = await _categoryService.GetAllAsync(userContext);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var userContext = HttpContext.GetUserContext();
            var response = await _categoryService.GetByIdAsync(id, userContext);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var response = await _categoryService.CreateAsync(createCategoryRequest);
            return Ok(response);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            var userContext = HttpContext.GetUserContext();
            var response = await _categoryService.UpdateAsync(id, updateCategoryRequest, userContext);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var userContext = HttpContext.GetUserContext();
            await _categoryService.DeleteAsync(id, userContext);
            return Ok();
        }
    }
}

using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Category;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ClaimsPrincipal _userContext;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _userContext = HttpContext.GetUserContext();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _categoryService.GetAllAsync(_userContext);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _categoryService.GetByIdAsync(id, _userContext);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var response = await _categoryService.CreateAsync(createCategoryRequest, _userContext);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            var response = await _categoryService.UpdateAsync(id, updateCategoryRequest, _userContext);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _categoryService.DeleteAsync(id, _userContext);
            return Ok();
        }
    }
}

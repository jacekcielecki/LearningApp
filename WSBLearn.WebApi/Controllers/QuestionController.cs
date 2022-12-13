using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Question;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetAllByCategoryAsync(int categoryId)
        {
            var response = await _questionService.GetAllByCategoryAsync(categoryId);
            return Ok(response);
        }

        [HttpGet("{categoryId}/{level}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetQuizAsync(int categoryId, [FromRoute] int level)
        {
            var userId = HttpContext.GetUserId();
            var response = await _questionService.GetQuizAsync(categoryId, level, userId);
            return Ok(response);
        }

        [HttpPost("{categoryId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionRequest request, int categoryId)
        {
            var response = await _questionService.CreateAsync(request, categoryId);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateQuestionRequest request)
        {
            var response = await _questionService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _questionService.DeleteAsync(id);
            return Ok();
        }
    }
}
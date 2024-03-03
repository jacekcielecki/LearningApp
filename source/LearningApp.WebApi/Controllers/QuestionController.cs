using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Question;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
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

        [HttpGet("all/{categoryId}/{level}")]
        public async Task<IActionResult> GetAllByLevelAsync(int categoryId, int level)
        {
            var response = await _questionService.GetAllByLevelAsync(categoryId, level);
            return Ok(response);
        }


        [HttpGet("{categoryId}/{level}")]
        public async Task<IActionResult> GetQuizAsync(int categoryId, [FromRoute] int level)
        {
            var response = await _questionService.GetQuizAsync(categoryId, level);
            return Ok(response);
        }

        [HttpPost("{categoryId}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionRequest request, int categoryId)
        {
            var response = await _questionService.CreateAsync(request, categoryId);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateQuestionRequest request)
        {
            var response = await _questionService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _questionService.DeleteAsync(id);
            return Ok();
        }
    }
}
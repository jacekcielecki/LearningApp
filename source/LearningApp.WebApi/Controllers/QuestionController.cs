using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.Question;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ClaimsPrincipal _userContext;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
            _userContext = HttpContext.GetUserContext();
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetAllByCategoryAsync(int categoryId)
        {
            var response = await _questionService.GetAllByCategoryAsync(categoryId, _userContext);
            return Ok(response);
        }

        [HttpGet("all/{categoryId}/{level}")]
        public async Task<IActionResult> GetAllByLevelAsync(int categoryId, int level)
        {
            var response = await _questionService.GetAllByLevelAsync(categoryId, level, _userContext);
            return Ok(response);
        }


        [HttpGet("{categoryId}/{level}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetQuizAsync(int categoryId, [FromRoute] int level)
        {
            var response = await _questionService.GetQuizAsync(categoryId, level, _userContext);
            return Ok(response);
        }

        [HttpPost("{categoryId}")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionRequest request, int categoryId)
        {
            var response = await _questionService.CreateAsync(request, categoryId, _userContext);
            return Ok(response);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateQuestionRequest request)
        {
            var response = await _questionService.UpdateAsync(id, request, _userContext);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _questionService.DeleteAsync(id, _userContext);
            return Ok();
        }
    }
}